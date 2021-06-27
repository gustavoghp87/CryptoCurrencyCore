using cryptoCurrency.Models;
using cryptoCurrency.Services.Blockchains;
using cryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace cryptoCurrency.Controllers
{
    [ApiController]
    [Route("api/")]
    [EnableCors("MyCors")]
    [Produces("application/json")]
    public class BlockchainController : ControllerBase
    {
        private Blockchain _blockchain;
        private IBlockchainService _blockchainServ;
        public BlockchainController(IBlockchainService blockchainService)
        {
            _blockchainServ = blockchainService;
            _blockchain = _blockchainServ.Get();
        }
        private void Update()
        {
            _blockchain = _blockchainServ.Get();
        }

        [HttpGet("blockchain")]
        public IActionResult Get()
        {
            Update();
            return Ok(_blockchain);
        }

        [HttpGet("blockchain/mine")]
        public async Task<IActionResult> Mine()
        {
            bool response = await _blockchainServ.Mine();
            if (!response) return BadRequest();
            Update();
            return Ok(_blockchain);
        }

        [HttpGet("blockchain/validation")]
        public IActionResult Validate()
        {
            Update();
            bool response = ValidateBlockchain.IsValid(_blockchain);
            return Ok(response);
        }
    }
}
