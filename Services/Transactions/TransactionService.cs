using Models;
using Services.Interfaces;
using Services.Wallets;
using System;
using System.Collections.Generic;

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
        public bool Add(Transaction transactionReq, Blockchain blockchain)
        {
            Transaction transaction = new();
            transaction.Amount = transactionReq.Amount;
            transaction.Fees = transactionReq.Fees;
            transaction.Miner = Miner.Wallet.PublicKey;
            transaction.Recipient = transactionReq.Recipient;
            transaction.Sender = transactionReq.Sender;
            transaction.Signature = transactionReq.Signature;
            transaction.Timestamp = transactionReq.Timestamp;
            transaction.Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(transactionReq.Timestamp);
            if (!IsTimeValidated(transaction.Timestamp)) return false;
            if (transactionReq.Amount < 0 || transaction.Fees < 0) return false;
            if (transactionReq.Amount == 0 && transaction.Fees == 0) return false;
            if (transactionReq.Sender == Issuer.Wallet.PublicKey && transactionReq.Amount > 0) return false;
            if (!WalletService.IsVerifiedMessage(transaction)) return false;
            bool success = Create(transaction, blockchain);
            // SendToNodes();
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
            return true;
        }
        private bool CheckJustOnePerTurn(Transaction transaction)
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
