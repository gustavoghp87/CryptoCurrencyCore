using Microsoft.AspNetCore.Mvc;

namespace CryptoCurrency.Controllers.Interfaces
{
    internal interface IWalletController
    {
        IActionResult GetBalance(string publicKey);
        IActionResult GetNew();
    }
}