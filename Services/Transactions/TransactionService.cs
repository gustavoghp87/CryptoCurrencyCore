using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private List<Transaction> _lstTransactions;
        private IBalanceService _balanceServ;
        private DateTime _actualDateTime;
        private readonly int _timeZone = 1;    // 1 minute
        public TransactionService(IBalanceService balanceServ)
        {
            _lstTransactions = new();
            _balanceServ = balanceServ;
        }
        public async Task<bool> Add(Transaction transactionReq)
        {
            Transaction transaction = new();
            transaction.Amount = transactionReq.Amount;
            transaction.Fees = transactionReq.Fees;
            transaction.Miner = Miner.MinerWallet.PublicKey;
            transaction.Recipient = transactionReq.Recipient;
            transaction.Sender = transactionReq.Sender;
            transaction.Signature = transactionReq.Signature;
            transaction.Timestamp = transactionReq.Timestamp;
            if (!IsTimeValidated(transaction.Timestamp)) return false;
            if (transactionReq.Amount < 0 || transaction.Fees < 0) return false;
            if (transactionReq.Amount == 0 && transaction.Fees == 0) return false;
            if (transactionReq.Sender == Issuer.IssuerWallet.PublicKey && transactionReq.Amount > 0) return false;
            if (!IsVerified(transaction)) return false;
            bool success = await Create(transaction);
            // SendToNodes();
            return success;
        }
        public List<Transaction> GetAll()
        {
            return _lstTransactions;
        }
        public void RenewDateTime(DateTime dateTime)
        {
            _actualDateTime = dateTime;
        }
        public void Clear()
        {
            _lstTransactions.Clear();
        }

        // private methods
        private static bool IsVerified(Transaction transaction)
        {
            return WalletService.IsVerifiedMessage(transaction);
        }
        private bool IsTimeValidated(DateTime timestamp)
        {
            //DateTime timeLimit = _actualDateTime.AddMinutes(_timeZone);
            //if (timestamp > _actualDateTime && timestamp < timeLimit) return true;
            //else return false;
            return true;
        }
        private async Task<bool> Create(Transaction transaction)
        {
            if (transaction.Sender == transaction.Recipient) return false;
            if (!CheckJustOnePerTurn(transaction)) return false;
            if (!await HasBalance(transaction)) return false;
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
        private async Task<bool> HasBalance(Transaction transaction)
        {
            if (transaction.Sender == Issuer.IssuerWallet.PublicKey) return true;    // limite this to issues

            // TODO: signature was used before ?
            int auxiliar = 0;
            foreach (Transaction aTransaction in _lstTransactions)
            {
                if (aTransaction.Signature == transaction.Signature
                    && aTransaction.Timestamp == transaction.Timestamp)
                {
                    auxiliar++;
                    if (auxiliar > 1) return false;
                }
            }
            _balanceServ.Initialize(transaction.Sender, _lstTransactions);
            decimal balance = await _balanceServ.GetAsync();
            return balance >= transaction.Amount + transaction.Fees;
        }
    }
}
