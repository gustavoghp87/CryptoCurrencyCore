using System.Collections.Generic;
using System.Threading.Tasks;

namespace cryptoCurrency.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<bool> Add(Models.Transaction transactionReq);
        void Clear();
        List<Models.Transaction> GetAll();
    }
}