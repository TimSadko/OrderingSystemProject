using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }
    
    private Bill? _current_bill = null;
    private Payment? _current_payment = null;

    public Bill? GetNewBill(int orderId)
    {
        /*
        var order = CommonRepository._order_rep.GetById(orderId);
        if (order == null)
            return null;
        order.Items = CommonRepository._order_item_rep.GetOrderItems(orderId);
        
        var bill = CommonRepository._bill_rep.GetByOrderId(orderId);

        if (bill == null)
        {
            bill = new Bill
            {
                Order = order,
                OrderId = order.OrderId,
            };
            CommonRepository._bill_rep.InsertBill(bill);
        }
        else
        {
            bill.Order = order;
        }

        _current_bill = bill;
        return bill;
        */
        
        var order = CommonRepository._order_rep.GetById(orderId);
        if (order == null)
            return null;
        
        order.Items = CommonRepository._order_rep.GetItemsForOrder(orderId) ?? new List<OrderItem>();
        if (order.Items.Count == 0)
            return null;

        var bill = CommonRepository._bill_rep.GetByOrderId(orderId);

        if (bill == null)
        {
            bill = new Bill
            {
                Order = order,
                OrderId = order.OrderId,
            };
            CommonRepository._bill_rep.InsertBill(bill);
        }
        else
        {
            bill.Order = order;
        }

        _current_bill = bill;
        return bill;
    }

    public Payment? GetNewPayment()
    {
        if (_current_bill == null)
            return null;

        var payment = new Payment
        {
            Bill = _current_bill,
            BillId = _current_bill.BillId
        };

        _current_payment = payment;
        return payment;
    }

    public List<Payment> SplitEqually(SplitEquallyViewModel splitEquallyViewModel)
    {
        decimal baseAmountPerPerson = CalculateBaseAmountPerPerson(splitEquallyViewModel);

        var payments = new List<Payment>();
        for (int i = 0; i < splitEquallyViewModel.NumberOfPeople; i++)
        {
            var payment = new Payment
            {
                BillId = splitEquallyViewModel.Bill.BillId,
                TipAmount = splitEquallyViewModel.TotalTip / splitEquallyViewModel.NumberOfPeople,
                PaymentAmount = baseAmountPerPerson + (splitEquallyViewModel.TotalTip / splitEquallyViewModel.NumberOfPeople),
                PaymentType = PaymentType.Cash,
                Feedback = "",
                Bill = splitEquallyViewModel.Bill
            };
            payments.Add(payment);
        }
        return payments;
    }
    

    private decimal CalculateBaseAmountPerPerson(SplitEquallyViewModel splitEquallyViewModel)
    {
        var bill = GetCurrentBill();
        decimal baseAmountPerPerson = bill.OrderTotal / splitEquallyViewModel.NumberOfPeople;
        return baseAmountPerPerson;
    }
    
    public void CloseBillAndFreeTable(int billId)
    {
        // Get bill
        var bill = GetBillById(billId);
        if (bill == null)
            throw new InvalidOperationException("Bill not found.");

        // Mark the associated order as completed
        var order = bill.Order;
        if (order != null)
        {
            foreach (var orderItem in order.Items)
            {
                CommonRepository._order_item_rep.UpdateOrderItemStatus(orderItem.Id, OrderItemStatus.Served);
            }
            CommonRepository._order_rep.UpdateOrderStatus(order.OrderId, OrderStatus.Completed);
        }

        // Free the table
        var table = order?.Table;
        if (table != null && order != null)
        {
            CommonRepository._tables_rep.UpdateTableStatus(order.TableId,TableStatus.Available);
        }
    }
    
    //GET SplitEqually
    public SplitEquallyViewModel BuildSplitEquallyViewModel(int billId)
    {
        var currentPayment = GetCurrentPayment();
        if (currentPayment != null)
        {
            billId = currentPayment.BillId;
        }

        var bill = GetBillById(billId);
        if (bill == null)
            return null;

        var model = new SplitEquallyViewModel
        {
            Bill = bill,
            NumberOfPeople = 2,
            Payments = null
        };

        InitializePayments(model); // helper method to initialize default payments

        return model;
    }

    //POST Update
    public void InitializePaymentsForUpdate(SplitEquallyViewModel splitEquallyViewModel)
    {
        InitializePayments(splitEquallyViewModel);
    }
    private void InitializePayments(SplitEquallyViewModel model)
    {
        if (model.Payments == null || model.Payments.Count != model.NumberOfPeople)
        {
            model.Payments = new List<Payment>();
            decimal perPersonAmount = Math.Round(model.Bill.OrderTotal / model.NumberOfPeople, 2);
        
            for (int i = 0; i < model.NumberOfPeople; i++)
            {
                model.Payments.Add(new Payment
                {
                    BillId = model.Bill.BillId,
                    PaymentAmount = perPersonAmount,
                    PaymentType = PaymentType.Cash,
                    TipAmount = 0,
                    Feedback = ""
                });
            }
        }
    }
    //POST SplitEqually
    public void InsertSplitPayments(List<Payment> payments)
    {
        foreach (var payment in payments)
        {
            if (payment.PaymentAmount <= 0)
            {
                throw new ArgumentException("Payment amount must be greater than zero.");
            }
            InsertPayment(payment);
        }
    }
    
    //GET SplitByAmount
    public SplitByAmountViewModel BuildSplitByAmountViewModel(int billId, string confirmationMessage = null)
    {
        var bill = GetBillById(billId);
        if (bill == null)
            return null;

        return new SplitByAmountViewModel
        {
            Bill = bill,
            ExistingPayments = GetPaymentsByBillId(billId),
            ConfirmationMessage = confirmationMessage
        };
    }
    //POST SplitByAmount
    public Bill GetValidatedBillForPayment(Payment payment)
    {
        var bill = GetBillById(payment.BillId);
        return bill;
    }

    public void CalculateTip(Payment payment, string selectedTipOption, string customTipString)
    {
        if (selectedTipOption == "custom" && decimal.TryParse(customTipString, out var parsedCustomTip))
        {
            payment.TipAmount = parsedCustomTip;
        }
        else if (decimal.TryParse(selectedTipOption, out var tipPercent))
        {
            payment.TipAmount = payment.PaymentAmount * tipPercent;
        }
        else
        {
            payment.TipAmount = 0;
        }
    }

    public bool ValidatePayment(Payment payment, out string validationError)
    {
        if (payment.PaymentAmount <= 0)
        {
            validationError = "Payment amount must be greater than zero.";
            return false;
        }

        validationError = null;
        return true;
    }

    public void PreparePaymentForSplit(Payment payment, Bill bill)
    {
        payment.IsFromSplitByAmount = true;
        payment.Bill = bill;
    }
    
    

    public Bill GetBillForPaymentById(Payment payment)
    {
        return CommonRepository._bill_rep.GetById(payment.BillId);
    }

    public decimal GetPaymentAmount(Payment payment)
    {
        return payment.Bill.OrderTotal + payment.TipAmount;
    }

    public void SetTipAmount(Payment payment)
    {
        if (payment.SelectedTipOption == "custom")
        {
            payment.TipAmount = payment.CustomTipAmount.Value;
        }
        else
        {
            if (decimal.TryParse(payment.SelectedTipOption, out var percent))
            {
                payment.TipAmount = payment.Bill.OrderTotal * percent;
            }
        }
    }

    public Bill? GetBillById(int billId)
    {
        return CommonRepository._bill_rep.GetById(billId);
    }

    public Payment InsertUpdatedPayment(Payment payment)
    {
        var insertedPayment = CommonRepository._payment_rep.InsertPayment(payment);
        return insertedPayment;
    }
    
    public Payment? GetById(int id)
    {
        return _paymentRepository.GetById(id);
    }

    public Payment InsertPayment(Payment payment)
    {
        return _paymentRepository.InsertPayment(payment);
    }

    public List<Payment> GetPaymentsByBillId(int billId)
    {
        return _paymentRepository.GetPaymentsByBillId(billId);
    }
    
    public Payment? GetCurrentPayment() { return _current_payment; }
    public Bill? GetCurrentBill() { return _current_bill; }
}