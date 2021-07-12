using CryptoCurrency.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoCurrency.Services.Interfaces
{
    public interface INodeService
    {
        List<Node> GetAll();
        Blockchain GetLongestBlockchain();
        Task RegisterMe();
        bool RegisterOne(Node node);
        void SendNewBlockchain(Blockchain newBlockchain);
    }
}
