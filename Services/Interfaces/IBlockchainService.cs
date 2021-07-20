using Models;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBlockchainService
    {
        Blockchain Get();
        Task<bool> Mine();
        bool ReceiveNew(Blockchain blockchain);
    }
}