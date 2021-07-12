using System;
using System.Numerics;
using System.Text;

namespace CryptoCurrency.Services.Blockchains
{
    public static class HashScore
    {
        private readonly static int _hashLength = 64;
        private readonly static int _maxScore = 35;
        private readonly static int _mode = 2;
        private readonly static string[] _characters = new string[]
        {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b",
            "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n",
            "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
        };
        public static string Get(string hash)
        {
            BigInteger score = GetScore(hash);
            int zeros = GetZeros(hash);
            BigInteger rest = GetRest(score, zeros);
            string text = GetRestInText(zeros, rest);
            return text.ToString();
        }
        private static BigInteger GetScore(string hash)
        {
            if (hash.Length != _hashLength) return 0;
            BigInteger score = 0;
            long i = 0;
            foreach (char c in hash.ToCharArray())
            {
                score += ScoreOneCharacter(c.ToString()) * (BigInteger)Math.Pow(_maxScore+1, _hashLength-1-i);
                i++;
            }
            return score;
        }
        private static int ScoreOneCharacter(string c)
        {
            for (int i = 0; i < _characters.Length; i++)
            {
                if (c == _characters[i]) return _maxScore - i;
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
            BigInteger rest = score;
            for (int i = 0; i < zeros; i++)
            {
                rest -= _maxScore * (BigInteger)Math.Pow(_maxScore+1, _hashLength-1-i);
            }
            return rest;
        }
        private static string GetRestInText(int zeros, BigInteger rest)
        {
            StringBuilder text = new();
            string restString = rest.ToString();
            text.Append(zeros.ToString() + " zeros + ");
            if (restString.Length > 3)
            {
                if (_mode == 1) text.Append(restString[0] + "." + restString.Substring(1,3) + " x10^");
                else text.Append(restString[0] + "." + restString.Substring(1,3) + "e+");
                text.Append(restString.Substring(1, restString.Length-1).Length);
            }
            else
            {
                text.Append(restString);
            }
            return text.ToString();
        }
    }
}