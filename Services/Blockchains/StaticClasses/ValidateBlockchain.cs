using Models;
using Services.Blocks;
using Services.Transactions;
using System.Linq;

namespace Services.Blockchains
{
    public static class BlockchainValidation
    {
        public static bool IsValid(Blockchain blockchain)
        {
            if (blockchain == null || blockchain.Blocks == null || blockchain.Blocks.Count == 0) return false;
            if (blockchain.IssuerWallet.PublicKey != Issuer.Wallet.PublicKey) return false;
            Block block1 = blockchain.Blocks.ElementAt(0);
            if (block1.PreviousHash != "null!") return false;
            if (!ValidateBlock.IsValid(block1)) return false;
            if (blockchain.Blocks.Count == 1) return true;
            for (int i = 1; i < blockchain.Blocks.Count; i++)
            {
                Block block = blockchain.Blocks.ElementAt(i);
                Block lastBlock = blockchain.Blocks.ElementAt(i - 1);
                lastBlock = ProofOfWorkService.Create(lastBlock);

                if (block.PreviousHash != lastBlock.Hash) return false;
                if (!ValidateBlock.IsValid(block)) return false;
                int issuerCounter = 0;
                if (block.Transactions == null || block.Transactions.Count == 0) return false;
                foreach(Transaction transaction in block.Transactions)
                {
                    if (transaction.Sender == Issuer.Wallet.PublicKey)
                    {
                        if (transaction.Amount != 0 && transaction.Fees != Reward.Get(i)) return false;
                        issuerCounter++;
                    }
                    if (!TransactionSignature.IsVerifiedMessage(transaction)) return false;
                }
                if (issuerCounter != 1) return false;
            }
            return true;
        }
    }
}
