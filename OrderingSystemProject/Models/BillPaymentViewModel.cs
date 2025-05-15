namespace OrderingSystemProject.Models;
using Microsoft.AspNetCore.Authorization;

public class BillPaymentViewModel
{
    public BillPaymentViewModel()
    {
        Bills = new List<Bill>();
        Orders = new List<Order>();
        Payment = null;
        AllBills = new List<Bill>();
        AllOrders = new List<Order>();
        AllPayments = new List<Payment>();
    }

    public BillPaymentViewModel(List<Bill> bills, List<Order> orders, Payment payment, List<Order> allOrders, List<Bill> allBills, List<Payment> allPayments)
    {
        Bills = bills;
        Orders = orders;
        Payment = payment;
        AllBills = allBills;
        AllOrders = allOrders;
        AllPayments = allPayments;
    }

    public List<Bill> Bills { get; set; }
    public List<Order> Orders { get; set; }
    public Payment? Payment { get; set; }
    public List<Order> AllOrders { get; set; }
    public List<Bill> AllBills { get; set; }
    public List<Payment> AllPayments { get; set; }
}