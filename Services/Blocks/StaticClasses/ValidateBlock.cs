using Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Services.Blocks
{
    public static class ValidateBlock
    {
        public static bool IsValid(Block block)
        {
            string hash = GetHash(block);
            string startsWith = "";
            for (int i = 1; i <= block.Difficulty; i++)
            {
                startsWith += "0";
            }
            bool success = hash.StartsWith(startsWith);
            if (success) Console.WriteLine("starts with: " + startsWith);
            return success;
        }
        public static Block GetBestHash(Block block){
            Stopwatch sw;
            long bestNonce = 0;
            string bestHash = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
            sw = Stopwatch.StartNew();
            for (int i = 0; i <= 10000; i++)
            {
                block.Nonce = i;
                string newHash = GetHash(block);
                string aux = GetHigher(bestHash, newHash);
                if (bestHash != aux)
                {
                    bestHash = aux;
                    bestNonce = i;
                }
                // if (i == 100 || i == 200 || i == 300 || i == 400 || i == 500 || i == 600 || i == 700 || i == 800 || i == 900 || i == 1000)
                //     Console.WriteLine(i + ": Hash: " + bestHash + ", Nonce: " + bestNonce);
            }
            sw.Stop();
            //Console.WriteLine(sw.ElapsedMilliseconds);
            block.Hash = bestHash;
            block.Nonce = (int)bestNonce;
            Console.WriteLine("Best Hash: " + bestHash + ", best Nonce: " + bestNonce);
            return block;
        }
        public static string GetHash(Block block)
        {
            string guess = GenerateMessage(block);
            byte[] bytes = Encoding.Unicode.GetBytes(guess);
            byte[] hash = new SHA256Managed().ComputeHash(bytes);
            var hashBuilder = new StringBuilder();
            foreach (byte x in hash)
                hashBuilder.Append($"{x:x2}");
            return hashBuilder.ToString();
        }
        
        private static string GetHigher(string hash1, string hash2)
        {
            string[] hashes = new string[] { hash1, hash2 };
            string[] ordered = (from x in hashes orderby x select x).ToArray();
            return ordered.ElementAt(0);
        }
        private static string GenerateMessage(Block block)
        {
            string signs = "";
            foreach (var transaction in block.Transactions)
            {
                signs += transaction.Signature;
            }
            // return $"{block.Index}-[{block.Timestamp:yyyy-MM-dd HH:mm:ss}]-{block.Nonce}-{block.PreviousHash}-{signs}";
            return $"{block.Index}-[{block.Timestamp}]-{block.Nonce}-{block.PreviousHash}-{signs}";
        }
            // TODO: Difficulty in message ??
    }
}
