using Microsoft.AspNetCore.Mvc;
using OrderingSystemProject.Models;
using OrderingSystemProject.Repositories;
using OrderingSystemProject.Services;

namespace OrderingSystemProject.Controllers;

public class PaymentController : Controller
{
    private readonly IDisplayOrderRepository _orderRepository;

    public PaymentController(IDisplayOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    [HttpGet ("Payment/Details/{id}")]
    public IActionResult Details(int id)
    {
        var orderSummary = _orderRepository.GetFullOrderSummary(id);
        if (orderSummary == null || orderSummary.Order == null)
        {
            return NotFound();
        }
        return View(orderSummary);
    }
}