using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.Services;
using OrderingSystemProject.Utilities;

namespace OrderingSystemProject.Controllers;

public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;
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
            var payment = _paymentService.GetNewPayment(); //gets new payment

            //checks if there are payments to display, if not creates new payment list
            if (billId.HasValue)
            {
                var payments = _paymentService.GetPaymentsByBillId(billId.Value);
                ViewBag.ExistingPayments = payments;
            }
            else
            {
                ViewBag.ExistingPayments = new List<Payment>();
            }

            //returns the payment to the view
            return View(payment);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "An error occurred while loading pay details.";
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
            var bill = _paymentService.GetNewBill(id); //gets new bill
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
            var model = _paymentService.BuildSplitEquallyViewModel(billId); //prepares the splitModel

            if (model == null)
            {
                TempData["ErrorMessage"] = "Bill not found.";
                return RedirectToAction("Overview", "Restaurant");
            }

            return View("SplitEqually", model);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "An error occurred while splitting. Please try again.";
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
            //check if the number of people to split is higher then 1
            if (model.NumberOfPeople < 1)
            {
                TempData["ErrorMessage"] = "Number of people must be greater than 0.";
                return View("SplitEqually", model);
            }

            var bill = _paymentService.GetCurrentBill(); //gets current bill
            model.Bill = bill; // assigns the current Bill to model Bill
            
            _paymentService.InitializePaymentsForUpdate(model); //initializes the existing payments to update them
            
            TempData["SuccessMessage"] = "Number of people updated successfully!";
            return View("SplitEqually", model);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "An error occurred while updating the number of people.";
            return RedirectToAction("Overview", "Restaurant");
        }
    }

    //Updated!
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
            
            //calculates the tip amount based on the number of people.
            _paymentService.CalculateEqualSplitPayments(bill, model.NumberOfPeople, model.Payments);

            //inserts the payments into the database
            _paymentService.InsertSplitPayments(model.Payments);
            TempData["SuccessMessage"] = "Split has been successfully done!";
            return RedirectToAction("Pay", new { billId = bill.BillId });
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An error occurred while splitting. Please try again.";
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
            var viewModel = _paymentService.BuildSplitByAmountViewModel(billId);

            if (viewModel == null)
            {
                TempData["ErrorMessage"] = "An error occurred while loading bill details.";
                return RedirectToAction("Overview", "Restaurant");
            }

            return View("SplitByAmount", viewModel);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "An error occurred while splitting. Please try again.";
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
            var payment = _paymentService.GetCurrentPayment(); //gets current payment
            if (payment != null) //checks if payment is not null
            {
                model.NewPayment.BillId = payment.BillId; //assigns the billId from the current payment,
            }

            var bill = _paymentService.GetValidatedBillForPayment(model.NewPayment); // gets the bill by the correct billId and check if the bill is not null
            
            if (bill == null)
            {
                TempData["ErrorMessage"] = "Bill not found.";
                return RedirectToAction("Overview", "Restaurant");
            }

            //calculates the Tip for the new Payment, selecting it from the form directly
            _paymentService.CalculateTip(model.NewPayment, Request.Form["SelectedTipOption"]!, Request.Form["CustomTipAmount"]!);

            //ModelState is giving the error in the view directly without changing the data that was prompted by the user.
            if (!_paymentService.ValidatePayment(model.NewPayment, out var validationError))
            {
                ModelState.AddModelError("NewPayment.PaymentAmount", validationError);
                model.Bill = bill;
                model.ExistingPayments = _paymentService.GetPaymentsByBillId(bill.BillId);
                return View("SplitByAmount", model);
            }

            //prepares the payment for the insert
            _paymentService.PreparePaymentForSplit(model.NewPayment, bill);
            
            //inserts the payment into the database
            _paymentService.InsertPayment(model.NewPayment);
            
            //displaying the success message if the payment is done correctly
            TempData["SuccessMessage"] = $"Payment of €{model.NewPayment.PaymentAmount + model.NewPayment.TipAmount:0.00} submitted successfully!";
            return RedirectToAction("SplitByAmount", new { billId = bill.BillId });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "An error occurred while splitting. Please try again.";
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
            payment.Bill = _paymentService.GetBillForPaymentById(payment); //gets the bill by id for payment

            if (payment.Bill == null)
            {
                TempData["ErrorMessage"] = "An error occurred while populating the bill.";
                throw new Exception("Bill not found for given BillId.");
            }
            
            _paymentService.SetTipAmount(payment); //sets the tip amount for the payment, whether it is custom or percentage
            payment.PaymentAmount = _paymentService.GetPaymentAmount(payment); //gets amount of the payment
            var insertedPayment = _paymentService.InsertUpdatedPayment(payment); //inserts the payment
            
            return RedirectToAction("Confirmation", new { id = insertedPayment.PaymentId });
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "An error occurred while submitting the payment. Please try again.";
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
            var payment = _paymentService.GetById(id); //gets the payment by the billId
            return View(payment);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "An error occurred while submitting the payment. Please try again.";
            return RedirectToAction("Overview", "Restaurant");
        }
    }
    [HttpPost]
    public IActionResult ConfirmAndCloseTheOrder(int billId)
    {
        if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role
        try
        {
            _paymentService.CloseBillAndFreeTable(billId); //closes the table based on the billId
            return RedirectToAction("Overview", "Restaurant");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "An error occurred while closing the order. Please try again.";
            return RedirectToAction("Pay", new { billId });
        }
    }

    [HttpGet ("Payment/FinishPaymentSplitByAmount/{billId}")]
    public IActionResult FinishPaymentSplitByAmount(int? billId)
    {
        if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role
        try
        {
            var latestPayment = _paymentService.GetCurrentPayment(); //current payment
            if (latestPayment != null)
            {
                billId = latestPayment.BillId;
            }

            if (!billId.HasValue) //checks whether billId has value and not null
            {
                return RedirectToAction("Overview", "Restaurant");
            }

            var payments = _paymentService.GetPaymentsByBillId(billId.Value); //gets payments by billId
            var bill = _paymentService.GetBillById(billId.Value);

            if (bill == null)
            {
                TempData["ErrorMessage"] = "Bill not found.";
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
            TempData["ErrorMessage"] = "An error occurred while finishing the payment. Please try again.";
            return RedirectToAction("Pay", new { billId });
        }
    }
    
    //Updated!
    [HttpPost]
    public IActionResult FinishPaymentForSplitByAmount(int billId)
    {
        if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role
        try
        {
            _paymentService.CloseBillAndFreeTable(billId); //closes the bill
            return RedirectToAction("Overview", "Restaurant");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "An error occurred while finishing the payment. Please try again.";
            return RedirectToAction("Pay", new { billId });
        }
    }
    [HttpPost]
    public IActionResult FinishPaymentForSplitEqually(int billId)
    {
        if (!Authenticate()) return RedirectToAction("Login", "Employees"); // check if user is logged in and has correct role
        try
        {
            _paymentService.CloseBillAndFreeTable(billId); //closes the bill
            return RedirectToAction("Overview", "Restaurant");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "An error occurred while finishing the payment. Please try again.";
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