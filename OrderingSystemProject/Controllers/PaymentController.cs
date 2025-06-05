using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.Services;

namespace OrderingSystemProject.Controllers;

public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;
    private const string SplitPaymentsKey = "SplitPayments";
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("Payment/Pay")]
    public IActionResult Pay(int? billId = null)
    {
        try
        {
            var payment = _paymentService.GetNewPayment();

            // If billId is passed, load payments for that bill
            if (billId.HasValue)
            {
                var payments = CommonRepository._payment_rep.GetPaymentsByBillId(billId.Value);
                ViewBag.ExistingPayments = payments;
            }
            else
            {
                ViewBag.ExistingPayments = new List<Payment>();
            }
            return View(payment);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return RedirectToAction("Overview", "Restaurant");
        }
    }
    
    [HttpGet ("Payment/Details/{id}")]
    public IActionResult Details(int id)
    {
        try
        {
            var bill = _paymentService.GetNewBill(id);
            return View(bill);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return RedirectToAction("Overview", "Restaurant");
        }
    }
    
    [HttpGet]
    public IActionResult SplitEqually(int id)
    {
        try
        {
            var bill = _paymentService.GetNewBill(id);
            if (bill == null)
                return RedirectToAction("Overview", "Restaurant");

            return View("SplitEqually", new SplitEquallyViewModel { Bill = bill });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("Overview", "Restaurant");
        }
    }

    [HttpPost]
    public IActionResult SplitEqually(SplitEquallyViewModel model)
    {
        try
        {
            if (model.NumberOfPeople < 1)
            {
                ModelState.AddModelError("NumberOfPeople", "Number of people must be at least 1.");
                return View(model);
            }

            // Re-fetch the bill with full details
            var bill = _paymentService.GetCurrentBill();
            if (bill?.Order?.Items == null)
            {
                throw new Exception("Bill or associated Order/Items not loaded.");
            }

            decimal baseAmountPerPerson = bill.OrderTotal / model.NumberOfPeople;

            var payments = new List<Payment>();
            for (int i = 0; i < model.NumberOfPeople; i++)
            {
                var payment = new Payment
                {
                    BillId = model.Bill.BillId,
                    TipAmount = model.ExtraTip / model.NumberOfPeople,
                    PaymentAmount = baseAmountPerPerson + (model.ExtraTip / model.NumberOfPeople),
                    PaymentType = PaymentType.Cash,
                    Feedback = "",
                    Bill = model.Bill
                };
                payments.Add(payment);
            }
            return View("SplitEqually", new SplitEquallyViewModel { Bill = bill, Payments = payments });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("Overview", "Restaurant");
        }
    }
    
    [HttpGet("Payment/ProcessSplitPayment/{billId}")]
    public IActionResult ProcessSplitPayment(int billId)
    {
        var bill = CommonRepository._bill_rep.GetById(billId);
        if (bill == null)
            return RedirectToAction("Overview", "Restaurant");

        var payments = CommonRepository._payment_rep.GetPaymentsByBillId(billId);
        var totalPaid = payments.Sum(p => p.PaymentAmount);
        var remainingAmount = bill.OrderTotal - totalPaid;

        //if (remainingAmount <= 0)
            //return RedirectToAction("Confirmation", new { id = payments.Last().PaymentId });

        var newPayment = new Payment
        {
            BillId = billId,
            Bill = bill,
            PaymentAmount = remainingAmount // this can be overridden by the user
        };

        ViewBag.Remaining = remainingAmount;
        return View("ProcessSplitPayment", newPayment);
    }

    [HttpPost("Payment/ProcessSplitPayment")]
    public IActionResult ProcessSplitPayment(Payment userPayment)
    {
        userPayment.Bill = CommonRepository._bill_rep.GetById(userPayment.BillId);

        if (userPayment.Bill == null)
            return RedirectToAction("Overview", "Restaurant");

        // Save individual payment to DB
        CommonRepository._payment_rep.InsertPayment(userPayment);

        var payments = CommonRepository._payment_rep.GetPaymentsByBillId(userPayment.BillId);
        var totalPaid = payments.Sum(p => p.PaymentAmount);

        if (totalPaid >= userPayment.Bill.OrderTotal)
        {
            return RedirectToAction("Confirmation", new { id = userPayment.PaymentId });
        }

        return RedirectToAction("ProcessSplitPayment", new { billId = userPayment.BillId });
    }
    
    [HttpPost]
    public IActionResult Pay(Payment payment)
    {
        try
        {
            Console.WriteLine($"payment is null: {payment == null}");
            Console.WriteLine($"payment.Bill is null: {payment?.Bill == null}");
            Console.WriteLine($"payment.SelectedTipAmount: {payment.SelectedTipAmount}");
            Console.WriteLine($"payment.Bill.OrderTotal: {payment?.Bill?.OrderTotal}");
            Console.WriteLine($"payment.TipAmount: {payment?.TipAmount}");
            Console.WriteLine($"payment.PaymentType: {payment?.PaymentType}");
            
            payment.Bill = CommonRepository._bill_rep.GetById(payment.BillId);

            if (payment.Bill == null)
            {
                throw new Exception("Bill not found for given BillId.");
            }
            
            //if (!ModelState.IsValid)
            //{
               // payment.Bill = CommonRepository._bill_rep.GetById(payment.BillId);
                //return View(payment);
            //}

            payment.TipAmount = payment.Bill.OrderTotal * payment.SelectedTipAmount;
            payment.PaymentAmount = payment.Bill.OrderTotal + payment.TipAmount;
            
            var insertedPayment = CommonRepository._payment_rep.InsertPayment(payment);
            
            return RedirectToAction("Confirmation", new { id = insertedPayment.PaymentId });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
    [HttpGet]
    public IActionResult Confirmation(int id)
    {
        try
        {
            var payment = CommonRepository._payment_rep.GetById(id);
            return View(payment);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return RedirectToAction("Overview", "Restaurant");
        }
    }
}