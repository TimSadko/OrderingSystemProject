using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.Services;

namespace OrderingSystemProject.Controllers;

public class PaymentController : Controller
{
    private readonly IOrdersRepository _ordersRepository;

    public PaymentController(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }
    
    [HttpGet ("Payment/Details/{id}")]
    public IActionResult Details(int id)
    {
        Bill bill = new Bill();
        var orderSummary = _ordersRepository.GetById(id);
        if (orderSummary == null)
        {
            return NotFound();
        }
        bill.Order = orderSummary;
        return View(bill);
    }
}