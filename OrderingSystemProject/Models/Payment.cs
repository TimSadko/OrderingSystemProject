using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OrderingSystemProject.Models
{
    public enum PaymentType
    {
        Cash = 0, DebitCard = 1, CreditCard = 2
    }
    
    public class Payment
    {
        public int PaymentId { get; set; }
        public int BillId { get; set; }

        public decimal TipAmount{ get; set; }
        
        [Required(ErrorMessage = "Please select a tip amount.")]
        //public decimal SelectedTipOption { get; set; }
        public string SelectedTipOption { get; set; }
        
        //enables only if the option custom is selected in the radio button selection
        public decimal? CustomTipAmount { get; set; }
        
        [Required(ErrorMessage = "Please choose a payment type.")] 
        public PaymentType PaymentType { get; set; }
        public decimal PaymentAmount{ get; set; }
        
        [StringLength(250, ErrorMessage = "Feedback cannot be longer than 250 characters.")]
        public string Feedback { get; set; }
        
        public Bill Bill { get; set; }
        
        public Payment()
        {
            PaymentId = 0;
            BillId = 0;
            TipAmount = 0;
            PaymentType = PaymentType.Cash;
            PaymentAmount = 0;
            Feedback = "";
        }

        public Payment(int paymentId, int billId, decimal tipAmount, PaymentType paymentType, decimal paymentAmount, string feedback)
        {
            PaymentId = paymentId;
            BillId = billId;
            TipAmount = tipAmount;
            PaymentType = paymentType;
            PaymentAmount = paymentAmount;
            Feedback = feedback;
        }
    }
}