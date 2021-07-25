using Microsoft.AspNetCore.Mvc;

namespace CryptoCurrency.Controllers.Interfaces
{
    internal interface INodeController
    {
        IActionResult AddNode();
        IActionResult GetNodes();
    }
}