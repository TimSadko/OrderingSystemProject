namespace OrderingSystemProject.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int BillId { get; set; }
        public int OrderId { get; set; }
        public decimal TipAmount { get; set; }
        public int PaymentType { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Feedback { get; set; }
        
        public Payment()
        {
            PaymentId = 0;
            BillId = 0;
            OrderId = 0;
            TipAmount = 0;
            PaymentType = 0;
            PaymentAmount = 0;
            Feedback = "";
        }

        public Payment(int paymentId, int billId, int orderId, decimal tipAmount, int paymentType, int paymentAmount, string feedback)
        {
            PaymentId = paymentId;
            BillId = billId;
            OrderId = orderId;
            TipAmount = tipAmount;
            PaymentType = paymentType;
            PaymentAmount = paymentAmount;
            Feedback = feedback;
        }
    }
}