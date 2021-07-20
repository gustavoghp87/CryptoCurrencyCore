using CryptoCurrency.Controllers.Interfaces;
using Models;
using Services.Blockchains;
using Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CryptoCurrency.Controllers
{
    [ApiController]
    [Route("api/")]
    [EnableCors("MyCors")]
    [Produces("application/json")]
    public class BlockchainController : ControllerBase, IBlockchainController
    {
        private Blockchain _blockchain;
        private readonly IBlockchainService _blockchainServ;
        public BlockchainController(IBlockchainService blockchainService)
        {
            _blockchainServ = blockchainService;
            _blockchain = _blockchainServ.Get();
        }

        [HttpGet("blockchain")]
        public IActionResult Get()
        {
            _blockchain = _blockchainServ.Get();
            return Ok(_blockchain);
        }

        [HttpPost("blockchain")]
        public IActionResult ReceiveNew(Blockchain blockchain)
        {
            bool response = _blockchainServ.ReceiveNew(blockchain);
            return Ok(response);
        }

        [HttpGet("blockchain/mine")]
        public async Task<IActionResult> Mine()
        {
            bool response = await _blockchainServ.Mine();
            if (!response) return BadRequest();
            Blockchain blockchain = _blockchainServ.Get();
            return Ok(blockchain);
        }

        [HttpGet("blockchain/validation")]
        public IActionResult Validate()
        {
            _blockchain = _blockchainServ.Get();
            bool response = ValidateBlockchain.IsValid(_blockchain);
            return Ok(response);
        }
    }
}
