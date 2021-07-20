using Models;
using Services.Blocks;
using Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Blockchains
{
    public partial class BlockchainService : IBlockchainService
    {
        private Blockchain _blockchain;
        private readonly Wallet _minerWallet;
        private readonly INodeService _nodeService;
        private readonly ITransactionService _transactionServ;
        private ISignTransactionService _signTransactionServ;
        public BlockchainService(INodeService nodeService, ITransactionService transactionServ,
            ISignTransactionService signTransactionService)
        {
            _blockchain = new();
            _minerWallet = Miner.MinerWallet;
            _nodeService = nodeService;
            _transactionServ = transactionServ;
            _blockchain.IssuerWallet = Issuer.IssuerWallet;
            _signTransactionServ = signTransactionService;
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
            _signTransactionServ.Initialize(transaction, _blockchain.IssuerWallet.PrivateKey);
            transaction.Signature = _signTransactionServ.GetSignature();
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
