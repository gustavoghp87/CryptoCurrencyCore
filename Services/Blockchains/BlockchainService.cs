using cryptoCurrency.Models;
using cryptoCurrency.Services.Blocks;
using cryptoCurrency.Services.Interfaces;
using cryptoCurrency.Services.Nodes;
using cryptoCurrency.Services.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cryptoCurrency.Services.Blockchains
{
    public partial class BlockchainService : IBlockchainService
    {
        private Blockchain _blockchain;
        private readonly Wallet _minerWallet;
        private readonly INodeService _nodeService;
        private readonly ITransactionService _transactionServ;
        public BlockchainService(INodeService nodeService, ITransactionService transactionServ)
        {
            _blockchain = new();
            _minerWallet = MinerService.Get();
            _nodeService = nodeService;
            _transactionServ = transactionServ;
            _blockchain.IssuerWallet = IssuerService.Get();
            Initialize();
        }
        private async void Initialize()
        {
            _blockchain.Nodes = _nodeService.GetAll();
            _nodeService.RegisterMe();
            Blockchain largestBC = _nodeService.GetLongestBlockchain();
            if (largestBC != null && largestBC.Blocks != null && largestBC.Blocks.Count != 0)
            {
                _blockchain = largestBC;
            }
            else
            {
                _blockchain.Blocks = new();
                await Mine();
            }
        }
        public async Task<bool> Mine()
        {
            bool response = await PayMeReward();
            if (!response) return false;
            List<Transaction> lstTransactions = _transactionServ.GetAll();
            Block lastBlock;
            Block newBlock;
            int newDifficulty = new NewDifficulty().Get();



            if (_blockchain.Blocks.Count != 0)
            {
                lastBlock = _blockchain.Blocks.Last();
                newBlock = new BlockService(lastBlock.Index + 1, lastBlock.Hash, lstTransactions, newDifficulty).GetMined();
            }
            else
            {
                newBlock = new BlockService(1, "null!", lstTransactions, newDifficulty).GetMined();
            }




            foreach (Block block in _blockchain.Blocks)            //  ???????
            {
                if (newBlock.Index == block.Index) return false;
            };
            
            newBlock.DifficultyT = HashScore.Get(newBlock.Hash);
            _blockchain.Blocks.Add(newBlock);
            _blockchain.LastDifficulty = newBlock.DifficultyT;
            
            
            // if (newBlock != null) SendToNodes();
            //_nodeServ.UpdateList();
            _transactionServ.Clear();
            return true;
        }
        private async Task<bool> PayMeReward()
        {
            Transaction transaction = new()
            {
                Amount = 0,
                Fees = GetReward(),
                Miner = _minerWallet.PublicKey,
                Recipient = _minerWallet.PublicKey,
                Sender = _blockchain.IssuerWallet.PublicKey,
                Timestamp = DateTime.UtcNow
            };
            SignTransactionService signServ = new(transaction, _blockchain.IssuerWallet.PrivateKey);
            transaction.Signature = signServ.GetSignature();
            return await _transactionServ.Add(transaction);
        }
        private decimal GetReward()                 // decrese rewards to a half every 100 blocks
        {
            decimal reward = 50;
            decimal auxiliar = _blockchain.Blocks != null ? _blockchain.Blocks.Count : 0;
            while (auxiliar / 100 > 1)
            {
                auxiliar /= 100;
                reward /= 2;
            }
            _blockchain.LastReward = reward;
            return reward;
        }
    }
}
