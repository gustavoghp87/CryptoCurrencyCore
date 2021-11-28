using Models;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBlockchainService
    {
        Blockchain Get();
        //void Initialize(string myIp);
        void Initialize();
        Task<bool> Mine();
        bool ReceiveNew(Blockchain blockchain);
    }
}