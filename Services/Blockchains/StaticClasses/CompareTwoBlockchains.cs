using Models;
using Services.Transactions;

namespace Services.Blockchains
{
    public static class CompareTwoBlockchains
    {
        public static bool IsLonger(Blockchain blockchain1, Blockchain blockchain2)
        {
            if (!AreValid(blockchain1, blockchain2)) return false;
            return blockchain1.Blocks.Count > blockchain2.Blocks.Count;
        }
        public static bool HaveAtLeastTheSameValidTransactions(Blockchain blockchain1, Blockchain blockchain2)
        {

            foreach (Block block1 in blockchain1.Blocks)
            {
                foreach (Block block2 in blockchain2.Blocks)
                {
                    if (block1.Index != block2.Index) continue;

                    if (block1.Transactions.Count >= block1.Transactions.Count) return false;

                    foreach (Transaction transaction in block1.Transactions)    // time problem - signature used before
                    {
                        bool isValid = TransactionSignature.IsVerifiedMessage(transaction);
                        if (!isValid) return false;
                    }
                }
            }
            return true;
        }
        public static bool IsBetter(Blockchain blockchain1, Blockchain blockchain2)
        {
            if (!AreValid(blockchain1, blockchain2)) return false;
            if (!IsLonger(blockchain1, blockchain2)) return false;
            foreach (Block block1 in blockchain1.Blocks)
            {
                foreach (Block block2 in blockchain2.Blocks)
                {
                    if (block1.Index != block2.Index) continue;
                    if (block1.Difficulty < block2.Difficulty) return false;
                }
            }
            return true;    // TODO: isBetter != isEqual
        }
        private static bool AreValid(Blockchain blockchain1, Blockchain blockchain2)
        {
            if (blockchain1 == null || blockchain2 == null || blockchain1.Blocks == null || blockchain2.Blocks == null)
                return false;
            else
                return true;
        }
    }
}