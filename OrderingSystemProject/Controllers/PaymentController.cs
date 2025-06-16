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
        if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role
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
        if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role

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
        if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role
        try
        {
            var model = _paymentService.BuildSplitEquallyViewModel(billId);

            if (model == null)
            {
                TempData["ErrorMessage"] = "Bill not found.";
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
        if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role
        try
        {
            if (model.NumberOfPeople < 1)
            {
                //ModelState.AddModelError("NumberOfPeople", "Number of people must be at least 1");
                TempData["ErrorMessage"] = "Number of people must be greater than 0.";
                return View("SplitEqually", model);
            }

            var bill = _paymentService.GetCurrentBill();
            model.Bill = bill;
            _paymentService.InitializePaymentsForUpdate(model);
            
            TempData["SuccessMessage"] = "Number of people updated successfully!";
            return View("SplitEqually", model);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("Overview", "Restaurant");
        }
    }

    //CHANGE!!!
    [HttpPost]
    public IActionResult SplitEqually(SplitEquallyViewModel model)
    {
        if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role
        try
        {
            var bill = _paymentService.GetCurrentBill();

            if (bill == null)
            {
                TempData["ErrorMessage"] = "Could not find the current bill.";
                return RedirectToAction("Overview", "Restaurant");
            }

            model.Bill = bill;

            decimal perPersonShare = bill.OrderTotal / model.NumberOfPeople;

            foreach (var payment in model.Payments)
            {
                if (payment.PaymentAmount > perPersonShare)
                {
                    payment.TipAmount = payment.PaymentAmount - perPersonShare;
                }
                else
                {
                    payment.TipAmount = 0;
                }
            }

            _paymentService.InsertSplitPayments(model.Payments);

            return RedirectToAction("Pay", new { billId = bill.BillId });
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Overview", "Restaurant");
        }
    }
    
    //Updated!
    [HttpGet("Payment/SplitByAmount/{billId}")]
    public IActionResult SplitByAmount(int billId)
    {
        if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role
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
        if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role
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
            return RedirectToAction("SplitByAmount", new { billId = bill.BillId });
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
        if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role
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
        if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role
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
    [HttpPost]
    public IActionResult ConfirmAndCloseTheOrder(int billId)
    {
        if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role
        try
        {
            _paymentService.CloseBillAndFreeTable(billId);
            return RedirectToAction("Overview", "Restaurant");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            TempData["ErrorMessage"] = e.Message;
            return RedirectToAction("Pay", new { billId });
        }
    }

    [HttpGet ("Payment/FinishPaymentSplitByAmount/{billId}")]
    public IActionResult FinishPaymentSplitByAmount(int? billId)
    {
        if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role
        try
        {
            var latestPayment = _paymentService.GetCurrentPayment();
            if (latestPayment != null)
            {
                billId = latestPayment.BillId;
            }

            if (!billId.HasValue)
            {
                return RedirectToAction("Overview", "Restaurant");
            }

            var payments = _paymentService.GetPaymentsByBillId(billId.Value);
            var bill = _paymentService.GetBillById(billId.Value);

            if (bill == null)
            {
                Console.WriteLine("Bill not found.");
                return RedirectToAction("Overview", "Restaurant");
            }

            var viewModel = new SplitByAmountViewModel
            {
                Bill = bill,
                ExistingPayments = payments,
                NewPayment = new Payment { BillId = bill.BillId }
            };

            return View("FinishPaymentSplitByAmount", viewModel);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    //Updated!
    [HttpPost]
    public IActionResult FinishPaymentForSplitByAmount(int billId)
    {
        if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role
        try
        {
            _paymentService.CloseBillAndFreeTable(billId);
            return RedirectToAction("Overview", "Restaurant");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            TempData["ErrorMessage"] = e.Message;
            return RedirectToAction("Pay", new { billId });
        }
    }
    private bool Authenticate()
    {
        var user_role = Authorization.GetUserRole(this.HttpContext);

        if (user_role != null && (user_role == EmployeeType.Waiter || user_role == EmployeeType.Manager)) return true;

        return false;
    }
}