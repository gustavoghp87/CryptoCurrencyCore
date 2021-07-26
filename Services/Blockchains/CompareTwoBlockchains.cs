using Models;

namespace Services.Blockchains
{
    public static class CompareTwoBlockchains
    {
        public static bool IsBetter(Blockchain blockchain1, Blockchain blockchain2)
        {
            if (blockchain1.Blocks.Count < blockchain2.Blocks.Count) return false;
            foreach (Block block1 in blockchain1.Blocks)
            {
                foreach (Block block2 in blockchain2.Blocks)
                {
                    if (block1.Index == block2.Index)
                    {
                        if (block1.Difficulty < block2.Difficulty) return false;
                    }
                }
            }
            return true;
        }
    }
}