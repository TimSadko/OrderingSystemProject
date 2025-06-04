using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.Services;

namespace OrderingSystemProject.Controllers;

public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("Payment/Pay")]
    public IActionResult Pay()
    {
        try
        {
            var payment = _paymentService.GetNewPayment();
            return View(payment);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            ViewData["Exception"] = e.Message;
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
            ViewData["Exception"] = e.Message;
            return RedirectToAction("Overview", "Restaurant");
        }
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
            ViewData["Exception"] = e.Message;
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
            ViewData["Exception"] = e.Message;
            return RedirectToAction("Overview", "Restaurant");
        }
    }
}