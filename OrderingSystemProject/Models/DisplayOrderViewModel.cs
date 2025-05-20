namespace OrderingSystemProject.Models;

public class DisplayOrderViewModel
{
    public Order? Order { get; set; }
    public List<OrderLineItemViewModel> OrderLines { get; set; } = new List<OrderLineItemViewModel>();
    public decimal OrderTotal { get; set; }
}