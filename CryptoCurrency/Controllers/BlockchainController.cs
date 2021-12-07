using CryptoCurrency.Controllers.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Blockchains;
using Services.Interfaces;
using System;

namespace CryptoCurrency.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("MyCors")]
    [Produces("application/json")]
    public class BlockchainController : ControllerBase, IBlockchainController
    {
        private readonly IBlockchainService _blockchainServ;
        public BlockchainController(IBlockchainService blockchainService)
        {
            _blockchainServ = blockchainService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            Blockchain blockchain = _blockchainServ.Get();
            Console.WriteLine("Getting blockchain -.-.-.----.");
            return Ok(blockchain);
        }

        [HttpPost]
        public IActionResult ReceiveNew(Blockchain blockchain)
        {
            bool response = _blockchainServ.ReceiveNew(blockchain);
            return Ok(response);
        }

        [HttpGet("mine")]
        public IActionResult Mine()
        {
            bool response = _blockchainServ.Mine();
            if (!response) return StatusCode(500);
            Blockchain blockchain = _blockchainServ.Get();
            return Ok(blockchain);
        }

        [HttpGet("validation")]
        public IActionResult Validate()
        {
            Blockchain blockchain = _blockchainServ.Get();
            bool response = BlockchainValidation.IsValid(blockchain);
            return Ok(response);
        }
    }
}
