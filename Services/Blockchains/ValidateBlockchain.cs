using Models;
using Services.Blocks;
using System.Linq;

namespace Services.Blockchains
{
    public static class ValidateBlockchain
    {
        public static bool IsValid(Blockchain blockchain)
        {
            if (blockchain == null || blockchain.Blocks == null || blockchain.Blocks.Count == 0) return false;
            if (blockchain.IssuerWallet.PublicKey != Issuer.IssuerWallet.PublicKey) return false;
            Block block1 = blockchain.Blocks.ElementAt(0);
            if (block1.PreviousHash != "null!") return false;
            if (!ValidateBlock.IsValid(block1)) return false;
            if (blockchain.Blocks.Count == 1) return true;
            for (int i = 1; i <= blockchain.Blocks.Count; i++)
            {
                Block block = blockchain.Blocks.ElementAt(i);
                Block lastBlock = blockchain.Blocks.ElementAt(i - 1);
                ProofOfWorkService powService = new(lastBlock);
                if (block.PreviousHash != powService.GetHash()) return false;
                if (!ValidateBlock.IsValid(block)) return false;
                int issuerCounter = 0;
                if (block.Transactions == null || block.Transactions.Count == 0) return false;
                foreach(Transaction transaction in block.Transactions)
                {
                    if (transaction.Sender == Issuer.IssuerWallet.PublicKey)
                    {
                        if (transaction.Amount != Reward.Get(i)) return false;
                        if (transaction.Fees != 0) return false;
                        issuerCounter++;
                    }
                    if (!WalletService.IsVerifiedMessage(transaction)) return false;
                }
                if (issuerCounter != 1) return false;
            }
            return true;
        }
    }
}
