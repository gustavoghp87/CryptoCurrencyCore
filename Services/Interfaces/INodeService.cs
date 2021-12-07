using Models;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface INodeService
    {
        List<Node> GetAll();
        Blockchain GetLongestBlockchain();
        void RegisterMe();
        bool RegisterOne(Node node);
        void SendNewBlockchain(Blockchain newBlockchain);
    }
}
