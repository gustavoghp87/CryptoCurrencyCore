using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace CryptoCurrency.Controllers.Interfaces
{
    internal interface ITransactionController
    {
        Task<IActionResult> AddTransaction(Transaction transactionRequest);
        IActionResult GetAll();
        IActionResult Sign(Transaction transactionRequest, string privateKey);
    }
}