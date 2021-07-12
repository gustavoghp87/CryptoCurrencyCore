using CryptoCurrency.Models;
using CryptoCurrency.Services.Blocks;
using CryptoCurrency.Services.Interfaces;
using CryptoCurrency.Services.Transactions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoCurrency.Services.Blockchains
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
            _minerWallet = Miner.Get();
            _nodeService = nodeService;
            _transactionServ = transactionServ;
            _blockchain.IssuerWallet = Issuer.Get();
            Initialize();
        }
        private async void Initialize()
        {
            _blockchain.Nodes = _nodeService.GetAll();
            var registerMe = _nodeService.RegisterMe();
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

            Block lastBlock = _blockchain.Blocks != null && _blockchain.Blocks.Count != 0 ? _blockchain.Blocks.Last() : null;
            Block newBlock = new BlockService(
                lastBlock != null ? lastBlock.Index + 1 : 1,
                lastBlock != null ? lastBlock.Hash : "null!",
                _transactionServ.GetAll(),
                new NewDifficulty().Get())
            .GetMined();

            if (newBlock == null) return false;
            newBlock.DifficultyT = HashScore.Get(newBlock.Hash);
            _blockchain.Blocks.Add(newBlock);
            _blockchain.LastDifficulty = newBlock.DifficultyT;
            _nodeService.SendNewBlockchain(_blockchain);
            _transactionServ.Clear();
            return true;
        }
        private async Task<bool> PayMeReward()
        {
            decimal reward = Reward.Get(_blockchain.Blocks != null ? _blockchain.Blocks.Count : 0);
            _blockchain.LastReward = reward;
            Transaction transaction = new()
            {
                Amount = 0,
                Fees = reward,
                Miner = _minerWallet.PublicKey,
                Recipient = _minerWallet.PublicKey,
                Sender = _blockchain.IssuerWallet.PublicKey,
                Timestamp = DateTime.UtcNow
            };
            SignTransactionService signServ = new(transaction, _blockchain.IssuerWallet.PrivateKey);
            transaction.Signature = signServ.GetSignature();
            return await _transactionServ.Add(transaction);
        }
        public Blockchain Get()
        {
            return _blockchain;
        }
        public bool ReceiveNew(Blockchain blockchain)
        {
            bool response1 = ValidateBlockchain.IsValid(blockchain);
            if (!response1) return false;
            bool response2 = CompareTwo.IsBetter(blockchain, _blockchain);
            if (!response2) return false;
            _blockchain = blockchain;
            return true;
        }
    }
}
