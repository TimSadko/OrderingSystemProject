using OrderingSystemProject.Models;

namespace OrderingSystemProject.Services;

public interface IPaymentService
{
    Bill? GetNewBill(int orderId);
    Payment? GetNewPayment();
    Payment? GetCurrentPayment();
    Bill? GetCurrentBill();
    List<Payment> SplitEqually(SplitEquallyViewModel splitEquallyViewModel);
    Payment InsertUpdatedPayment(Payment payment);
    Bill GetBillForPaymentById(Payment payment);
    decimal GetPaymentAmount(Payment payment);
    void SetTipAmount(Payment payment);

    Bill? GetBillById(int billId);
    void CloseBillAndFreeTable(int billId);
    //GET SplitEqually
    SplitEquallyViewModel BuildSplitEquallyViewModel(int billId);
    
    //POST UpdateNumberOfPeople
    void InitializePaymentsForUpdate(SplitEquallyViewModel splitEquallyViewModel);
    //POST SplitEqually
    void InsertSplitPayments(List<Payment> payments);
    void CalculateEqualSplitPayments(Bill bill, int numberOfPeople, List<Payment> payments);
    
    //GET SplitByAmount
    SplitByAmountViewModel BuildSplitByAmountViewModel(int billId);
    
    //POST SplitByAmount
    Bill GetValidatedBillForPayment(Payment payment);
    void CalculateTip(Payment payment, string selectedTipOption, string customTipString);
    bool ValidatePayment(Payment payment, out string validationError);
    void PreparePaymentForSplit(Payment payment, Bill bill);
    
    //methods from the repo
    public Payment? GetById(int id);
    Payment InsertPayment(Payment payment);
    List<Payment> GetPaymentsByBillId(int billId);
}