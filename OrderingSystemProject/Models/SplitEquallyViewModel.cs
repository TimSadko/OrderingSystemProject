namespace OrderingSystemProject.Models;

public class SplitEquallyViewModel
{
    public Bill Bill { get; set; }
    public List<Payment> Payments { get; set; }
    public int NumberOfPeople { get; set; }
    public decimal TotalTip { get; set; }
    public decimal PerPersonAmount { get; set; }
}