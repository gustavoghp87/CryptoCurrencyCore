using Models;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBlockchainService
    {
        Blockchain Get();
        bool Mine();
        bool ReceiveNew(Blockchain blockchain);
    }
}