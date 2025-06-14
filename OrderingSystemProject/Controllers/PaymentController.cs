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
    public IActionResult Pay()
    {
        try
        {
            var payment = _paymentService.GetNewPayment();

            // If billId is passed, load payments for that bill
            
            //Only needed when there are multiple payment from splitting part.
            /*
            if (billId.HasValue)
            {
                var payments = CommonRepository._payment_rep.GetPaymentsByBillId(billId.Value);
                ViewBag.ExistingPayments = payments;
            }
            else
            {
                ViewBag.ExistingPayments = new List<Payment>();
            }
            */
            return View(payment);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            ViewData["Exception"] = e.Message;
            return RedirectToAction("Overview", "Restaurant");
        }
    }
    
    //checked!!
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
    
    /*
     Before changes!!
      */
    [HttpGet]
    public IActionResult SplitEqually(int id)
    {
        try
        {
            /*
            var bill = _paymentService.GetNewBill(id);
            if (bill == null)
                return RedirectToAction("Overview", "Restaurant");
            
            var model = new SplitEquallyViewModel
            {
                Bill = bill,
                Payments = new List<Payment>(),
                NumberOfPeople = 2
            };
            return View("SplitEqually", model);
            */
            
            var bill = _paymentService.GetNewBill(id);
            if (bill == null)
                return RedirectToAction("Overview", "Restaurant");

            int numberOfPeople = 2;
            decimal perPersonAmount = bill.OrderTotal / numberOfPeople;

            var payments = new List<Payment>();
            for (int i = 0; i < numberOfPeople; i++)
            {
                payments.Add(new Payment
                {
                    BillId = bill.BillId,
                    PaymentAmount = perPersonAmount
                });
            }

            var model = new SplitEquallyViewModel
            {
                Bill = bill,
                NumberOfPeople = numberOfPeople,
                Payments = payments
            };

            return View("SplitEqually", model);

            //return View("SplitEqually", new SplitEquallyViewModel { Bill = bill });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("Overview", "Restaurant");
        }
    }

    //Checked!!
    [HttpPost]
    public IActionResult SplitEqually(SplitEquallyViewModel model)
    {
        try
        {
            /*
            if (model.NumberOfPeople < 1)
            {
                Console.WriteLine("Number of people is less than 1");
                return View(model);
            }

            // Re-fetch the bill with full details
            
            var bill = _paymentService.GetCurrentBill();
            var payments = _paymentService.SplitEqually(model);
            
            foreach (var payment in model.Payments)
            {
                _paymentService.InsertPayment(payment); // Save to repo
            }

            return RedirectToAction("Pay", new { billId = model.Bill.BillId });
            */
            
            if (model.NumberOfPeople < 1)
            {
                Console.WriteLine("Number of people is less than 1");
                return View(model);
            }

            var bill = _paymentService.GetCurrentBill();
            model.Bill = bill;

            var payments = _paymentService.SplitEqually(model);

            foreach (var payment in payments)
            {
                _paymentService.InsertPayment(payment);
            }

            return RedirectToAction("Pay", new { billId = bill.BillId });
            
            //return RedirectToAction("Pay");
            //return View("SplitEqually", new SplitEquallyViewModel { Bill = bill, Payments = payments });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("Overview", "Restaurant");
        }
    }
    
    /*
    [HttpPost]
    public IActionResult SplitEqually(SplitEquallyViewModel model)
    {
        try
        {
            if (model.NumberOfPeople < 1)
            {
                ModelState.AddModelError(nameof(model.NumberOfPeople), "Number of people must be at least 1.");
                var bill2 = _paymentService.GetCurrentBill();
                model.Bill = bill2;
                return View(model);
            }

            var bill = _paymentService.GetCurrentBill();
            var payments = _paymentService.SplitEqually(model);

            var perPersonAmount = (bill.OrderTotal + model.TotalTip) / model.NumberOfPeople;

            return View("SplitEqually", new SplitEquallyViewModel
            {
                Bill = bill,
                Payments = payments,
                NumberOfPeople = model.NumberOfPeople,
                TotalTip = model.TotalTip,
                PerPersonAmount = perPersonAmount
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("Overview", "Restaurant");
        }
    }
    */
    
    //!!!Needs Change!!!
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
            PaymentAmount = remainingAmount
        };

        ViewBag.Remaining = remainingAmount;
        return View("ProcessSplitPayment", newPayment);
    }

    //!!!Needs Change!!!
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
    
    //Checked!!
    [HttpPost]
    public IActionResult Pay(Payment payment)
    {
        try
        {
            payment.Bill = _paymentService.GetBillForPaymentById(payment);

            if (payment.Bill == null)
            {
                throw new Exception("Bill not found for given BillId.");
            }
            
            _paymentService.SetTipAmount(payment);
            payment.PaymentAmount = _paymentService.GetPaymentAmount(payment);
            var insertedPayment = _paymentService.InsertUpdatedPayment(payment);
            
            return RedirectToAction("Confirmation", new { id = insertedPayment.PaymentId });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            ViewData["Exception"] = e.Message;
            throw;
        }
    }
    
    //Checked!!
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
    
    //confirm for split payments
    [HttpPost]
    public IActionResult ConfirmSplitPayments(SplitEquallyViewModel model)
    {
        try
        {
            foreach (var payment in model.Payments)
            {
                _paymentService.InsertPayment(payment); // Save to repo
            }

            return RedirectToAction("Pay", new { billId = model.Bill.BillId });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            ViewData["Exception"] = e.Message;
            return RedirectToAction("Overview", "Restaurant");
        }
    }
    
    [HttpPost]
    public IActionResult FinishPayment(int billId)
    {
        try
        {
            //_paymentService.CloseBillAndFreeTable(billId);
            return RedirectToAction("Overview", "Restaurant");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            ViewData["Exception"] = e.Message;
            return RedirectToAction("Pay", new { billId });
        }
    }
}