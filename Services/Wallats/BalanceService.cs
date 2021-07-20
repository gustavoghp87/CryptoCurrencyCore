using Models;
using Newtonsoft.Json;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Services.Blockchains
{
    public class BalanceService : IBalanceService
    {
        private string _senderPublicKey;
        private ICollection<Transaction> _lstCurrentTransactions;
        private Blockchain _blockchain;
        public void Initialize(string publicKey, ICollection<Transaction> lstCurrentTransactions, Blockchain blockchain = null)
        {
            _senderPublicKey = publicKey;
            _lstCurrentTransactions = lstCurrentTransactions;
            _blockchain = blockchain;
        }
        public decimal Get()
        {
            return Process();
        }
        public async Task<decimal> GetAsync()
        {
            if (ThereIsValidBlockchain()) _blockchain = await GetBlockchain();
            if (ThereIsValidBlockchain()) return -1;
            return Process();
        }
        private decimal Process()
        {
            List<Transaction> lstTransactions = GetTransactionsByAddress();
            decimal balance = 0;
            string issuerAddress = Issuer.IssuerWallet.PublicKey;
            foreach (Transaction aTransaction in lstTransactions)
            {
                if (_senderPublicKey == aTransaction.Recipient)
                    balance += aTransaction.Amount;
                else if (_senderPublicKey == aTransaction.Sender)
                    balance -= aTransaction.Amount;
                if (_senderPublicKey == aTransaction.Miner)
                    balance += aTransaction.Fees;
                if (_senderPublicKey == issuerAddress)
                    balance -= aTransaction.Fees;
            }
            return balance;
        }
        private bool ThereIsValidBlockchain(){
            return _blockchain == null || _blockchain.Blocks == null || _blockchain.Blocks.Count == 0;
        }
        private List<Transaction> GetTransactionsByAddress()
        {
            List<Transaction> lstTransactions = new();
            //List<Transaction> lstCurrentTransactions = await GetCurrentTransactions();
            List<Block> lstBlocks = (from x in _blockchain.Blocks select x).ToList();
            foreach (var block in lstBlocks.OrderByDescending(x => x.Index))
            {
                List<Transaction> ownerTransactions =
                    block.Transactions
                    .Where(x => x.Sender == _senderPublicKey || x.Recipient == _senderPublicKey || x.Miner == _senderPublicKey)
                    .ToList();
                lstTransactions.AddRange(ownerTransactions);
            }
            foreach (var aTransaction in _lstCurrentTransactions)
            {
                if (aTransaction.Recipient == _senderPublicKey || aTransaction.Miner == _senderPublicKey)
                    lstTransactions.Add(aTransaction);
            }
            return lstTransactions;
        }
        private static async Task<Blockchain> GetBlockchain()
        {
            Blockchain blockchain;
            using var httpResponse = await new HttpClient().GetAsync("https://localhost:5001/", HttpCompletionOption.ResponseHeadersRead);
            httpResponse.EnsureSuccessStatusCode();
            if (httpResponse.Content is object && httpResponse.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = await httpResponse.Content.ReadAsStreamAsync();
                using var streamReader = new StreamReader(contentStream);
                using var jsonReader = new JsonTextReader(streamReader);
                JsonSerializer serializer = new();
                try
                {
                    blockchain = serializer.Deserialize<Blockchain>(jsonReader);
                }
                catch (JsonReaderException)
                {
                    Console.WriteLine("Invalid JSON.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("HTTP Response was invalid and cannot be deserialised.");
                return null;
            }
            return blockchain;
        }
        // private static async Task<List<Transaction>> GetCurrentTransactions()
        // {
        //     List<Transaction> currentTransactions;
        //     using var httpResponse = await new HttpClient().GetAsync("https://localhost:5001/transaction",
        //                                                                 HttpCompletionOption.ResponseHeadersRead);
        //     httpResponse.EnsureSuccessStatusCode();
        //     if (httpResponse.Content is object && httpResponse.Content.Headers.ContentType.MediaType == "application/json")
        //     {
        //         var contentStream = await httpResponse.Content.ReadAsStreamAsync();
        //         using var streamReader = new StreamReader(contentStream);
        //         using var jsonReader = new JsonTextReader(streamReader);
        //         JsonSerializer serializer = new();
        //         try
        //         {
        //             currentTransactions = serializer.Deserialize<List<Transaction>>(jsonReader);
        //         }
        //         catch (JsonReaderException)
        //         {
        //             Console.WriteLine("Invalid JSON.");
        //             return null;
        //         }
        //     }
        //     else
        //     {
        //         Console.WriteLine("HTTP Response was invalid and cannot be deserialised.");
        //         return null;
        //     }
        //     return currentTransactions;
        // }
    }
}
