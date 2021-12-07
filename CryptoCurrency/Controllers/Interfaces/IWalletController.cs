using Microsoft.AspNetCore.Mvc;

namespace CryptoCurrency.Controllers.Interfaces
{
    internal interface IWalletController
    {
        IActionResult GetBalance(string publicKey);
        IActionResult GetIssuerBalance();
        IActionResult GetMinerBalance();
        IActionResult GetNew();
        IActionResult GetTransactions(string publicKey);
    }
}