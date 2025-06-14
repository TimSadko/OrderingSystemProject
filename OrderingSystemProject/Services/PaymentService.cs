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

    public void GetBillById(int billId)
    {
        CommonRepository._bill_rep.GetById(billId);
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