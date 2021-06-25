using BlockchainAPI.Models;
using BlockchainAPI.Services.Blocks;
using BlockchainAPI.Services.Nodes;
using BlockchainAPI.Services.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlockchainAPI.Services.Blockchains
{
    public partial class BlockchainService : IBlockchainService
    {
        private void SendToNodes()
        {
            foreach (Node node in _blockchain.Nodes)
            {
                new HttpClient().PostAsJsonAsync(node.ToString() + "/new-blockchain", _blockchain);
            }
        }
        public Blockchain Get()
        {
            return _blockchain;
        }
        public TransactionService GetTransactionService()
        {
            return _transactionServ;
        }
    }
}
