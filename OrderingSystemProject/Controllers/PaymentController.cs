using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.Services;

namespace OrderingSystemProject.Controllers;

public class PaymentController : Controller
{
    private readonly IPaymentRepository _paymentRepository;
    private Bill _current_bill;

    public PaymentController(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    [HttpGet("Payment/Pay")]
    public IActionResult Pay()
    {
        Payment payment = new Payment();
        payment.Bill = _current_bill;
        return View(payment);
    }
    [HttpGet ("Bill/Details/{id}")]
    public IActionResult Details(int id)
    {
        _current_bill = new Bill();
        var orderSummary = CommonRepository._order_rep.GetById(id);
        if (orderSummary == null)
        {
            return NotFound();
        }
        _current_bill.Order = orderSummary;
        return View(_current_bill);
    }
    /*
    [HttpPost]
    public IActionResult Pay(Payment payment)
    {
        if (payment.ButtonPressed == "C")
        {
            payment.Input = "";
        }
        else if (payment.ButtonPressed == "<")
        {
            if (!string.IsNullOrEmpty(payment.Input))
                payment.Input = payment.Input.Substring(0, payment.Input.Length - 1);
        }
        else
        {
            payment.Input += payment.ButtonPressed;
        }
    }
    */
}