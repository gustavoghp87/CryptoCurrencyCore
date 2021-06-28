using cryptoCurrency.Models;
using cryptoCurrency.Services.Transactions;
using System.Threading.Tasks;

namespace cryptoCurrency.Services.Interfaces
{
    public interface IBlockchainService
    {
        Blockchain Get();
        Task<bool> Mine();
        bool ReceiveNew(Blockchain blockchain);
    }
}