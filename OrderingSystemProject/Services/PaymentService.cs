using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;

namespace OrderingSystemProject.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrdersRepository _ordersRepository;
    private readonly IBillRepository _billRepository;
    private readonly IOrderItemsRepository _orderItemsRepository;
    private readonly ITablesRepository _tablesRepository;

    public PaymentService(IPaymentRepository paymentRepository, IOrdersRepository ordersRepository, IBillRepository billRepository, IOrderItemsRepository orderItemsRepository, ITablesRepository tablesRepository)
    {
        _paymentRepository = paymentRepository;
        _ordersRepository = ordersRepository;
        _billRepository = billRepository;
        _orderItemsRepository = orderItemsRepository;
        _tablesRepository = tablesRepository;
    }
    
    private Bill? _current_bill = null;
    private Payment? _current_payment = null;

    public Bill? GetNewBill(int orderId)
    {
        //gets order by id
        var order = _ordersRepository.GetById(orderId);
        if (order == null)
            return null;
        //gets the order items by order id
        order.Items = order.Items;
        if (order.Items.Count == 0)
            return null;
        
        //gets bill by id
        var bill = _billRepository.GetByOrderId(orderId);

        if (bill == null)
        {
            bill = new Bill
            {
                Order = order,
                OrderId = order.OrderId,
            };
            _billRepository.InsertBill(bill);
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

        //makes new payment remaining existing bill id, and the info of the bill
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

        //creates new list of payments
        var payments = new List<Payment>();
        
        //loops the number of selected people and creates new payments with the data from the viewModel, also calculates values for each payment.
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
        //calculates the amount each person has to pay
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
                _orderItemsRepository.UpdateOrderItemStatus(orderItem.Id, OrderItemStatus.Served);
            }
            _ordersRepository.UpdateOrderStatus(order.OrderId, OrderStatus.Completed);
        }

        // Free the table
        var table = order?.Table;
        if (table != null && order != null)
        {
            _tablesRepository.UpdateTableStatus(order.TableId,TableStatus.Available);
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

        //creates base model
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
            //calculates the per person amount for the split based on the base info, so for 2
            decimal perPersonAmount = Math.Round(model.Bill.OrderTotal / model.NumberOfPeople, 2);
        
            for (int i = 0; i < model.NumberOfPeople; i++)
            {
                //adds the base data into the payment
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
        //inserts the payments into the database
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
    public SplitByAmountViewModel BuildSplitByAmountViewModel(int billId)
    {
        var bill = GetBillById(billId);
        if (bill == null)
            return null;

        return new SplitByAmountViewModel
        {
            Bill = bill,
            ExistingPayments = GetPaymentsByBillId(billId),
        };
    }
    //POST SplitByAmount
    public Bill GetValidatedBillForPayment(Payment payment)
    {
        var bill = GetBillById(payment.BillId);
        if (bill == null)
        {
            throw new InvalidOperationException("Bill not found.");
        }
        return bill;
    }

    public void CalculateTip(Payment payment, string selectedTipOption, string customTipString)
    {
        //parses the data in case the custom tip is selected
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
    
    //
    public void CalculateEqualSplitPayments(Bill bill, int numberOfPeople, List<Payment> payments)
    {
        if (bill == null) throw new ArgumentNullException(nameof(bill));
        if (numberOfPeople <= 0) throw new ArgumentException("Number of people must be greater than zero.");
        if (payments == null || payments.Count == 0) throw new ArgumentException("No payments provided.");

        decimal perPersonShare = bill.OrderTotal / numberOfPeople;

        foreach (var payment in payments)
        {
            payment.TipAmount = payment.PaymentAmount > perPersonShare
                ? payment.PaymentAmount - perPersonShare
                : 0;
        }
    }
    
    public Bill GetBillForPaymentById(Payment payment)
    {
        return _billRepository.GetById(payment.BillId);
    }

    public decimal GetPaymentAmount(Payment payment)
    {
        return payment.Bill.OrderTotal + payment.TipAmount;
    }

    public void SetTipAmount(Payment payment) //is used for pay
    {
        if (payment.SelectedTipOption == "custom")
        {
            if (payment.CustomTipAmount != null) payment.TipAmount = payment.CustomTipAmount.Value;
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
        return _billRepository.GetById(billId);
    }

    public Payment InsertUpdatedPayment(Payment payment)
    {
        var insertedPayment = _paymentRepository.InsertPayment(payment);
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