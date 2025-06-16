using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.Services;
using OrderingSystemProject.Utilities;

namespace OrderingSystemProject.Controllers;

public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;
    private const string SplitPaymentsKey = "SplitPayments";
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    //Updated!
    [HttpGet("Payment/Pay/{billId}")]
    public IActionResult Pay(int? billId)
    {
        try
        {
            var payment = _paymentService.GetNewPayment();

            if (billId.HasValue)
            {
                var payments = _paymentService.GetPaymentsByBillId(billId.Value);
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
            ViewData["ErrorMessage"] = e.Message;
            return RedirectToAction("Overview", "Restaurant");
        }
    }
    
    //checked!!
    [HttpGet ("Payment/Details/{id}")]
    public IActionResult Details(int id)
    {
        EmployeeType? userRole = Authorization.GetUserRole(HttpContext);
        // check if user is logged in and has correct role
        if (userRole != EmployeeType.Waiter && userRole != EmployeeType.Manager) return RedirectToAction("Login", "Employees");

        try
        {
            var bill = _paymentService.GetNewBill(id);
            if (bill == null)
            {
                TempData["ErrorMessage"] = "Bill not found.";
                return RedirectToAction("Overview", "Restaurant");
            }
            return View(bill);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "An error occurred while loading bill details.";
            return RedirectToAction("Overview", "Restaurant");
        }
    }
    
    //Updated!
    [HttpGet]
    public IActionResult SplitEqually(int billId)
    {
        try
        {
            var model = _paymentService.BuildSplitEquallyViewModel(billId);

            if (model == null)
            {
                Console.WriteLine("Bill not found.");
                return RedirectToAction("Overview", "Restaurant");
            }

            return View("SplitEqually", model);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("Overview", "Restaurant");
        }
    }
    
    //Updated!
    [HttpPost]
    public IActionResult UpdateNumberOfPeople(SplitEquallyViewModel model)
    {
        try
        {
            if (model.NumberOfPeople < 1)
            {
                ModelState.AddModelError("NumberOfPeople", "Number of people must be at least 1");
                return View("SplitEqually", model);
            }

            var bill = _paymentService.GetCurrentBill();
            model.Bill = bill;
            _paymentService.InitializePaymentsForUpdate(model);
            return View("SplitEqually", model);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("Overview", "Restaurant");
        }
    }

    //Updated
    [HttpPost]
    public IActionResult SplitEqually(SplitEquallyViewModel model)
    {
        try
        {
            var bill = _paymentService.GetCurrentBill();

            if (bill == null)
            {
                TempData["ErrorMessage"] = "Could not find the current bill.";
                return RedirectToAction("Overview", "Restaurant");
            }
            
            model.Bill = bill;
            
            _paymentService.InsertSplitPayments(model.Payments);

            return RedirectToAction("Pay", new { billId = bill.BillId });
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Overview", "Restaurant");
        }
    }
    
    /*
    private void InitializePayments(SplitEquallyViewModel model)
    {
        if (model.Payments == null || model.Payments.Count != model.NumberOfPeople)
        {
            model.Payments = new List<Payment>();
            decimal perPersonAmount = model.Bill.OrderTotal / model.NumberOfPeople;
            for (int i = 0; i < model.NumberOfPeople; i++)
            {
                model.Payments.Add(new Payment
                {
                    BillId = model.Bill.BillId,
                    PaymentAmount = perPersonAmount
                });
            }
        }
    }
    */
    
    //Updated!
    [HttpGet("Payment/SplitByAmount/{billId}")]
    public IActionResult SplitByAmount(int billId)
    {
        try
        {
            var viewModel = _paymentService.BuildSplitByAmountViewModel(billId, TempData["ConfirmationMessage"] as string);

            if (viewModel == null)
            {
                Console.WriteLine("Bill not found.");
                return RedirectToAction("Overview", "Restaurant");
            }

            return View("SplitByAmount", viewModel);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            ViewData["ErrorMessage"] = e.Message;
            return RedirectToAction("Overview", "Restaurant");
        }
    }

    //Updated!
    [HttpPost]
    public IActionResult SplitByAmount(SplitByAmountViewModel model)
    {
        try
        {
            var payment = _paymentService.GetCurrentPayment();
            if (payment != null)
            {
                model.NewPayment.BillId = payment.BillId;
            }

            var bill = _paymentService.GetValidatedBillForPayment(model.NewPayment);
            
            if (bill == null)
            {
                TempData["ErrorMessage"] = "Bill not found.";
                return RedirectToAction("Overview", "Restaurant");
            }

            _paymentService.CalculateTip(model.NewPayment, Request.Form["SelectedTipOption"], Request.Form["CustomTipAmount"]);

            if (!_paymentService.ValidatePayment(model.NewPayment, out var validationError))
            {
                ModelState.AddModelError("NewPayment.PaymentAmount", validationError);
                model.Bill = bill;
                model.ExistingPayments = _paymentService.GetPaymentsByBillId(bill.BillId);
                return View("SplitByAmount", model);
            }

            _paymentService.PreparePaymentForSplit(model.NewPayment, bill);
            _paymentService.InsertPayment(model.NewPayment);

            TempData["ConfirmationMessage"] = $"Payment of €{model.NewPayment.PaymentAmount + model.NewPayment.TipAmount:0.00} submitted successfully!";
            return RedirectToAction("Pay", new { billId = bill.BillId });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            TempData["ErrorMessage"] = "An error occurred while processing the payment.";
            return RedirectToAction("Overview", "Restaurant");
        }
    }
    
    //Updated!
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
            TempData["ErrorMessage"] = e.Message;
            return RedirectToAction("Overview", "Restaurant");
        }
    }
    
    //Updated!
    [HttpGet]
    public IActionResult Confirmation(int id)
    {
        try
        {
            var payment = _paymentService.GetById(id);
            return View(payment);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            TempData["ErrorMessage"] = e.Message;
            return RedirectToAction("Overview", "Restaurant");
        }
    }

    [HttpGet]
    public IActionResult FinishPaymentSplitByAmount(int? billId)
    {
        var payment = _paymentService.GetCurrentPayment();
        if (billId.HasValue)
        {
            var payments = _paymentService.GetPaymentsByBillId(billId.Value);
            ViewBag.ExistingPayments = payments;
        }
        else
        {
            ViewBag.ExistingPayments = new List<Payment>();
        }

        return View(payment);
    }
    
    //Updated!
    [HttpPost]
    public IActionResult FinishPaymentForSplitByAmount(int billId)
    {
        try
        {
            _paymentService.CloseBillAndFreeTable(billId);
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