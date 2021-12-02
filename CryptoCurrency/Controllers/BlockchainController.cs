using CryptoCurrency.Controllers.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Blockchains;
using Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace CryptoCurrency.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("MyCors")]
    [Produces("application/json")]
    public class BlockchainController : ControllerBase, IBlockchainController
    {
        private Blockchain _blockchain;
        private readonly IBlockchainService _blockchainServ;
        public BlockchainController(IBlockchainService blockchainService)
        {
            _blockchainServ = blockchainService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _blockchain = _blockchainServ.Get();
            Console.WriteLine("Getting blockchain -.-.-.----.");
            return Ok(_blockchain);
        }

        [HttpPost]
        public IActionResult ReceiveNew(Blockchain blockchain)
        {
            bool response = _blockchainServ.ReceiveNew(blockchain);
            return Ok(response);
        }

        [HttpGet("mine")]
        public async Task<IActionResult> Mine()
        {
            bool response = await _blockchainServ.Mine();
            if (!response) return StatusCode(500);
            Blockchain blockchain = _blockchainServ.Get();
            return Ok(blockchain);
        }

        [HttpGet("validation")]
        public IActionResult Validate()
        {
            _blockchain = _blockchainServ.Get();
            bool response = BlockchainValidation.IsValid(_blockchain);
            return Ok(response);
        }
    }
}
