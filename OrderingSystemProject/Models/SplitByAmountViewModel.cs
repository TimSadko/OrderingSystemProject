namespace OrderingSystemProject.Models;

public class SplitByAmountViewModel
{
    public Bill Bill { get; set; }
    public Payment NewPayment { get; set; } = new Payment();
    public List<Payment> ExistingPayments { get; set; } = new List<Payment>();
    public string ConfirmationMessage { get; set; }
}