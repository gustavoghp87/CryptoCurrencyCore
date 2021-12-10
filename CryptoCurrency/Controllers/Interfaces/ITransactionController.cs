using Microsoft.AspNetCore.Mvc;
using Models;

namespace CryptoCurrency.Controllers.Interfaces
{
    internal interface ITransactionController
    {
        IActionResult AddTransaction(Transaction transaction);
        IActionResult GetAll();
        IActionResult Sign(TransactionRequest transactionRequest);
    }
}