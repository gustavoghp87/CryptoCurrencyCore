using System;
namespace Services.Blockchains
{
    public static class Reward
    {
        public static decimal Get(long numberOfBlock)
        {
            if (numberOfBlock < 1) throw(new Exception("Non-valid number of block"));
            if (numberOfBlock >= 1200) return 0;
            decimal reward = 50;                   // starting reward
            int divisor = 100;                     // decrese rewards every 100 blocks
            int partition = 2;                     // to a half
            decimal auxiliar = numberOfBlock;
            while (auxiliar > divisor)
            {
                auxiliar -= divisor;
                reward /= partition;
            }
            return reward;
        }
    }
}