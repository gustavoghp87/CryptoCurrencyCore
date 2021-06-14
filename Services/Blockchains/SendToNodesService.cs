using BlockchainAPI.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace BlockchainAPI.Services.Blockchains
{
    public static class SendToNodesService
    {
        public static void Send(Node[] nodes, Blockchain blockchain)
        {
            foreach (Node node in nodes)
            {
                new HttpClient().PostAsJsonAsync(node.ToString() + "/new-blockchain", blockchain);
            }
        }
    }
}
