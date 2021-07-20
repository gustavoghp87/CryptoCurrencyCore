using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services.Interfaces
{
    public interface IBalanceService
    {
        decimal Get();
        Task<decimal> GetAsync();
        void Initialize(string publicKey, ICollection<Transaction> lstCurrentTransactions, Blockchain blockchain = null);
    }
}
