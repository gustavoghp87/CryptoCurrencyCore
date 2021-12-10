using Models;
using Services.Blocks;
using Services.Interfaces;
using Services.Transactions;
using System;
using System.Linq;

namespace Services.Blockchains
{
    public partial class BlockchainService : IBlockchainService
    {
        public static string DomainName { get; set; }
        private readonly INodeService _nodeService;
        private readonly ITransactionService _transactionServ;
        private readonly Wallet _minerWallet;
        private Blockchain _blockchain;
        public BlockchainService(INodeService nodeService, ITransactionService transactionServ)
        {
            _blockchain = new();
            _blockchain.IssuerWallet = Issuer.Wallet;
            _minerWallet = Miner.Wallet;
            _nodeService = nodeService;
            _nodeService.RegisterMe();
            _blockchain.Nodes = _nodeService.GetAll();
            _transactionServ = transactionServ;
            Blockchain largestBC = _nodeService.GetLongestBlockchain();
            if (largestBC != null && largestBC.Blocks != null && largestBC.Blocks.Count > 0)
            {
                _blockchain = largestBC;
                Console.WriteLine("#######################  Taken blockchain from net ####################### Running on " + DomainName);
            }
            else
            {
                Console.WriteLine("#######################  Building new blockchain  ####################### Running on " + DomainName);
                _blockchain.Blocks = new();
                Mine();
            }
        }
        public Blockchain Get()
        {
            return _blockchain;
        }
        public bool Mine()
        {
            bool response = PayMeReward();
            if (!response) return false;

            Block lastBlock = _blockchain.Blocks != null && _blockchain.Blocks.Count != 0 ? _blockchain.Blocks.Last() : null;
            Block newBlock = new BlockService(
                lastBlock != null ? lastBlock.Index + 1 : 1,
                lastBlock != null ? lastBlock.Hash : "null!",
                _transactionServ.GetAll(),
                NewDifficulty.Get())
            .GetMined();

            if (newBlock == null) return false;
            newBlock.DifficultyScore = HashScore.Get(newBlock.Hash);
            if (_blockchain.Blocks == null) return false;
            _blockchain.Blocks.Add(newBlock);
            _blockchain.LastDifficulty = newBlock.DifficultyScore;
            _blockchain.Nodes = _nodeService.GetAll();
            _nodeService.SendNewBlockchain(_blockchain);
            _transactionServ.Clear();
            return true;
        }
        public bool ReceiveNew(Blockchain blockchain)
        {
            bool isValid = BlockchainValidation.IsValid(blockchain);
            if (!isValid) return false;
            bool isBetter = CompareTwoBlockchains.IsBetter(blockchain, _blockchain);
            if (!isBetter) return false;
            _blockchain = blockchain;
            return true;
        }

        #region private methods region    ///////////////////////////////////////////////////////////////////////
        private bool PayMeReward()
        {
            decimal reward = Reward.Get(_blockchain.Blocks != null ? _blockchain.Blocks.Count + 1 : 1);
            _blockchain.LastReward = reward;
            long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Transaction transaction = new()
            {
                Amount = 0,
                Fees = reward,
                Miner = _minerWallet.PublicKey,
                Recipient = _minerWallet.PublicKey,
                Sender = _blockchain.IssuerWallet.PublicKey,
                Timestamp = timestamp,
                Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(timestamp)
            };
            transaction.Signature = TransactionSignature.Sign(transaction, _blockchain.IssuerWallet.PrivateKey);
            return _transactionServ.Add(transaction, _blockchain);
        }
        #endregion
    }
}
