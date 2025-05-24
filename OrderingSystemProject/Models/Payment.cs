namespace OrderingSystemProject.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int BillId { get; set; }
        public decimal TipAmount{ get; set; }

        public int PaymentType { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Feedback { get; set; }
        
        public Bill Bill { get; set; }
        
        //TO SHO VISRAL CHAT
        public string Input { get; set; } = ""; // raw input string (e.g., "1234" → represents 12.34)

        
         public decimal AmountTendered
        {
            get
            {
                var tendered = decimal.TryParse(string.IsNullOrEmpty(Input) ? "0.00" : Input.Length == 1 ? "0.0" + Input : Input.Length == 2 ? "0." + Input : Input.Insert(Input.Length - 2, "."),
                    out decimal amountTendered) ? amountTendered : 0m;
                return tendered;
            }
        }

        public string ButtonPressed { get; set; }
        
        public Payment()
        {
            PaymentId = 0;
            BillId = 0;
            TipAmount = 0;
            PaymentType = 0;
            PaymentAmount = 0;
            Feedback = "";
        }

        public Payment(int paymentId, int billId, decimal tipAmount, int paymentType, decimal paymentAmount, string feedback)
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