using Models;
using NBitcoin;
using System.Collections.Generic;
using System.Linq;
using Transaction = Models.Transaction;

namespace Services.Wallets
{
    public static class WalletService
    {
        public static decimal GetBalance(string senderPublicKey, Blockchain blockchain, List<Transaction> currentTransactions)
        {
            List<Transaction> transactionsList = GetTransactionsByAddress(senderPublicKey, blockchain, currentTransactions);
            if (transactionsList == null) return -1;
            decimal balance = 0;
            string issuerAddress = Issuer.Wallet.PublicKey;
            foreach (Transaction aTransaction in transactionsList)
            {
                if (senderPublicKey == aTransaction.Recipient)
                    balance += aTransaction.Amount;
                else if (senderPublicKey == aTransaction.Sender)
                {
                    balance -= aTransaction.Amount;
                    balance -= aTransaction.Fees;
                }
                if (senderPublicKey == aTransaction.Miner)
                    balance += aTransaction.Fees;
                //if (senderPublicKey == issuerAddress)
                //    balance -= aTransaction.Fees;
            }
            return balance;
        }
        public static List<Transaction> GetTransactionsByAddress(string senderPublicKey, Blockchain blockchain, List<Transaction> transactions)
        {
            if (blockchain == null || blockchain.Blocks == null || blockchain.Blocks.Count == 0) return null;
            List<Transaction> ownerTransactionsList = new();
            foreach (var block in blockchain.Blocks.OrderByDescending(x => x.Index))
            {
                List<Transaction> ownerTransactions =
                    block.Transactions
                    .Where(x => x.Sender == senderPublicKey || x.Recipient == senderPublicKey || x.Miner == senderPublicKey)
                    .ToList();
                ownerTransactionsList.AddRange(ownerTransactions);
            }
            if (transactions == null || transactions.Count == 0) return ownerTransactionsList;
            foreach (var aTransaction in transactions)
            {
                if (aTransaction.Recipient == senderPublicKey || aTransaction.Miner == senderPublicKey)
                    ownerTransactionsList.Add(aTransaction);
            }
            return ownerTransactionsList;
        }
        public static Wallet Generate()
        {
            Wallet newWallet = new();
            var bitcoinKey = new Key().GetBitcoinSecret(Network.Main);
            newWallet.PrivateKey = bitcoinKey.ToString();
            newWallet.PublicKey = bitcoinKey.GetAddress(ScriptPubKeyType.Legacy).ToString();
            newWallet.BitcoinAddress = BitcoinAddress.Create(newWallet.PublicKey, Network.Main).ToString();
            return newWallet;
        }
    }
}
