using CryptoCurrency.Models;
using CryptoCurrency.Services.Transactions;
using System.Threading.Tasks;

namespace CryptoCurrency.Services.Interfaces
{
    public interface IBlockchainService
    {
        Blockchain Get();
        Task<bool> Mine();
        bool ReceiveNew(Blockchain blockchain);
    }
}