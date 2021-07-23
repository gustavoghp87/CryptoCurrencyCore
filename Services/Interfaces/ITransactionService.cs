using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITransactionService
    {
        Task<bool> Add(Models.Transaction transactionReq);
        void Clear();
        List<Models.Transaction> GetAll();
        void RenewDateTime(DateTime dateTime);
    }
}