using Microsoft.AspNetCore.Mvc;
using Models;

namespace CryptoCurrency.Controllers.Interfaces
{
    internal interface IBlockchainController
    {
        IActionResult Get();
        IActionResult Mine();
        IActionResult ReceiveNew(Blockchain blockchain);
        IActionResult Validate();
    }
}