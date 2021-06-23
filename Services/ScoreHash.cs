using System;

namespace cryptoCurrency.Services
{
    public static class ScoreHash
    {
        private static string[] _characters = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c" };

        public static int Score(string hash)
        {
            if (hash.Length != 64) return 0;
            int score = 0;
            int i = 0;
            foreach (char c in hash)
            {
                score += ((int)Math.Pow(10, (64-i))) * ScoreOneCharacter(c.ToString());
            }
            return score;
        }

        private static int ScoreOneCharacter(string c)
        {
            switch (c)
            {
                case "0":
                    return 35;
                case "1":
                    return 34;
                case "2":
                    return 33;
                case "3":
                    return 32;
                case "4":
                    return 31;
                case "5":
                    return 30;
                case "6":
                    return 29;
                case "7":
                    return 28;
                case "8":
                    return 27;
                case "9":
                    return 26;
                case "a":
                    return 25;
                case "b":
                    return 24;
                case "c":
                    return 23;
                case "d":
                    return 22;
                case "e":
                    return 21;
                case "f":
                    return 20;
                case "g":
                    return 19;
                case "h":
                    return 18;
                case "i":
                    return 17;
                case "j":
                    return 16;
                case "k":
                    return 15;
                case "l":
                    return 14;
                case "m":
                    return 13;
                case "n":
                    return 12;
                case "o":
                    return 11;
                case "p":
                    return 10;
                case "q":
                    return 9;
                case "r":
                    return 8;
                case "s":
                    return 7;
                case "t":
                    return 6;
                case "u":
                    return 5;
                case "v":
                    return 4;
                case "w":
                    return 3;
                case "x":
                    return 2;
                case "y":
                    return 1;
                case "z":
                    return 0;
                default:
                    return 0;
            }
        }
    }
}