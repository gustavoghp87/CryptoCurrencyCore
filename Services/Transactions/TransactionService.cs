using Models;
using Services.Interfaces;
using Services.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private List<Transaction> _lstTransactions;
        // private DateTime _actualDateTime;
        // private readonly int _timeZone = 1;    // 1 minute
        public TransactionService()
        {
            _lstTransactions = new();
        }
        public bool Add(Transaction transaction, Blockchain blockchain)
        {
            transaction.Miner = Miner.Wallet.PublicKey;
            //Date = TimeService.TakeDateFromUnitTime(transaction.Timestamp)
            if (!IsTimeValidated(transaction.Timestamp)) return false;
            if (transaction.Amount < 0 || transaction.Fees < 0) return false;
            if (transaction.Amount == 0 && transaction.Fees == 0) return false;
            if (transaction.Sender == Issuer.Wallet.PublicKey && transaction.Amount > 0) return false;
            if (!TransactionSignature.IsVerifiedMessage(transaction)) return false;
            bool success = Create(transaction, blockchain);
            return success;
        }
        public List<Transaction> GetAll()
        {
            return _lstTransactions;
        }
        public void RenewDateTime(DateTime dateTime)
        {
            // _actualDateTime = dateTime;
        }
        public void Clear()
        {
            _lstTransactions.Clear();
        }

        // private methods
        private bool IsTimeValidated(long timestamp)
        {
            //DateTime timeLimit = _actualDateTime.AddMinutes(_timeZone);
            //if (timestamp > _actualDateTime && timestamp < timeLimit) return true;
            //else return false;
            return true;
        }
        private bool Create(Transaction transaction, Blockchain blockchain)
        {
            if (transaction.Sender == transaction.Recipient) return false;
            if (!CheckJustOnePerTurn(transaction)) return false;
            if (!HasBalance(transaction, blockchain)) return false;
            _lstTransactions.Add(transaction);
            _lstTransactions = _lstTransactions.OrderBy(x => x.Timestamp).ToList();
            return true;
        }
        private bool CheckJustOnePerTurn(Transaction transaction)    // TODO: instead, signature not repeated
        {
            foreach (var aTransaction in _lstTransactions)
            {
                if (aTransaction.Sender == transaction.Sender) return false;
            }
            return true;
        }
        private bool HasBalance(Transaction transaction, Blockchain blockchain)
        {
            if (transaction.Sender == Issuer.Wallet.PublicKey) return true;    // limite this to issues

            // TODO: signature was used before ?
            int auxiliar = 0;
            foreach (Transaction aTransaction in _lstTransactions)
            {
                if (aTransaction.Signature == transaction.Signature && aTransaction.Timestamp == transaction.Timestamp)
                {
                    auxiliar++;
                    if (auxiliar > 1) return false;
                }
            }
            decimal balance = WalletService.GetBalance(transaction.Sender, blockchain, _lstTransactions);
            return balance >= transaction.Amount + transaction.Fees;
        }
    }
}
