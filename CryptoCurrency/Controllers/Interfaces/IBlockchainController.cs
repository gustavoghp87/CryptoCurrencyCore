using Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CryptoCurrency.Controllers.Interfaces
{
    internal interface IBlockchainController
    {
        IActionResult Get();
        Task<IActionResult> Mine();
        IActionResult ReceiveNew(Blockchain blockchain);
        IActionResult Validate();
    }
}