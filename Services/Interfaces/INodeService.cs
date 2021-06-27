using cryptoCurrency.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cryptoCurrency.Services.Interfaces
{
    public interface INodeService
    {
        List<Node> GetAll();
        Blockchain GetLongestBlockchain();
        Task RegisterMe();
        bool RegisterOne(string address);
        void SendNewBlockchain(Blockchain newBlockchain);
        void UpdateList();
    }
}
