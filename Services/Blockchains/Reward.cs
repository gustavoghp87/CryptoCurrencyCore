namespace Services.Blockchains
{
    public static class Reward
    {
        public static decimal Get(int blocksCounter)              // decrese rewards to a half every 100 blocks
        {
            decimal reward = 50;
            decimal auxiliar = blocksCounter;
            while (auxiliar / 100 > 1)
            {
                auxiliar /= 100;
                reward /= 2;
            }
            return reward;
        }
    }
}