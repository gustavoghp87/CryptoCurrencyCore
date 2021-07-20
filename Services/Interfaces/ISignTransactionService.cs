using Models;

namespace Services.Interfaces
{
    public interface ISignTransactionService
    {
        string GetMessage();
        string GetSignature();
        void Initialize(Transaction transaction, string privateKey);
    }
}