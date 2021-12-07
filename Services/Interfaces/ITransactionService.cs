using Models;
using System;
using System.Collections.Generic;
using Transaction = Models.Transaction;

namespace Services.Interfaces
{
    public interface ITransactionService
    {
        bool Add(Transaction transactionReq, Blockchain blockchain);
        void Clear();
        List<Transaction> GetAll();
        void RenewDateTime(DateTime dateTime);
    }
}