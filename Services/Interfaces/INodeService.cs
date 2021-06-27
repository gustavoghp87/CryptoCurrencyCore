using cryptoCurrency.Models;
using System.Collections.Generic;

namespace cryptoCurrency.Services.Interfaces
{
    public interface INodeService
    {
        List<Node> GetAll();
        Blockchain GetLongestBlockchain();
        void RegisterMe();
        bool RegisterOne(string address);
        void SendNewBlockchain(Blockchain newBlockchain);
        void UpdateList();
    }
}
