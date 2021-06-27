using cryptoCurrency.Models;
using cryptoCurrency.Services.Blocks;
using System.Linq;

namespace cryptoCurrency.Services.Blockchains
{
    public static class ValidateBlockchainService
    {
        public static bool IsValid(Blockchain blockchain)
        {
            if (blockchain == null || blockchain.Blocks == null || blockchain.Blocks.Count == 0) return false;
            if (blockchain.IssuerWallet.PublicKey != IssuerService.Get().PublicKey) return false;
            
            Block block1 = blockchain.Blocks.ElementAt(0);
            if (block1.PreviousHash != "null!") return false;
            if (!ValidateBlockService.IsValid(block1)) return false;

            // TODO: validate transactions
            // TODO: validate monetary issue in transactions
            
            if (blockchain.Blocks.Count == 1) return true;

            for (int i = 1; i < blockchain.Blocks.Count; i++)
            {
                Block block = blockchain.Blocks.ElementAt(i);
                Block lastBlock = blockchain.Blocks.ElementAt(i - 1);
                ProofOfWorkService powService = new(lastBlock);
                if (block.PreviousHash != powService.GetHash()) return false;
                if (!ValidateBlockService.IsValid(block)) return false;
                blockchain.Blocks[i].Transactions.ForEach(block =>
                {
                    
                });
            }
            return true;
        }
    }
}
