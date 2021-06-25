using System;
using System.Numerics;

namespace cryptoCurrency.Services
{
    public static class HashScore
    {
        private static int hashLength = 64;
        private static int maxScore = 35;
        private static string[] _characters = new string[]
        {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e" ,
            "f" , "g" , "h" , "i" , "j" , "k" , "l" , "m" , "n" , "o" , "p" , "q" , "r" ,
            "s" , "t" , "u" , "v" , "w" , "x" , "y" , "z"
        };
        public static string Get(string hash)
        {
            BigInteger score = Score(hash);
            int zeros = GetZeros(hash);
            BigInteger rest = GetRest(score, zeros);
            return zeros.ToString() + " zeros + " + rest.ToString();
        }
        public static BigInteger Score(string hash)
        {
            if (hash.Length != hashLength) return 0;
            BigInteger score = 0;
            long i = 0;
            foreach (char c in hash.ToCharArray())
            {
                score += ScoreOneCharacter(c.ToString()) * (BigInteger)Math.Pow(maxScore+1, hashLength-1-i);
                i++;
            }
            return score;
        }
        private static int ScoreOneCharacter(string c)
        {
            for (int i = 0; i < _characters.Length; i++)
            {
                if (c == _characters[i]) return maxScore - i;
            }
            return 0;
        }
        private static int GetZeros(string hash)
        {
            int zeros = 0;
            foreach (char digit in hash.ToCharArray())
            {
                if (digit.ToString() == "0") zeros += 1;
                else return zeros;
            }
            return zeros;
        }
        private static BigInteger GetRest(BigInteger score, int zeros)
        {
            for (int i = 0; i < zeros; i++)
            {
                score -= maxScore * (BigInteger)Math.Pow(maxScore+1, hashLength-1-i);
            }
            return score;
        }
    }
}