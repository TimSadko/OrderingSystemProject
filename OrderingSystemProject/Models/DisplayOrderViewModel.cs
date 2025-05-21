namespace OrderingSystemProject.Models;

public class DisplayOrderViewModel
{
    public Order? Order { get; set; }
    public List<OrderLineItemViewModel> OrderLines { get; set; } = new List<OrderLineItemViewModel>();
    public decimal OrderSubtotal { get; set; }
    public decimal OrderTotal { get; set; }
    public decimal Vat { get; set; }
}