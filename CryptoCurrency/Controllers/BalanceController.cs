using Models;
using Services.Blockchains;
using Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CryptoCurrency.Controllers
{
    [ApiController]
    [Route("api/")]
    [EnableCors("MyCors")]
    [Produces("application/json")]
    public class BalanceController : ControllerBase
    {  
        private IBlockchainService _blockchainServ;
        private ITransactionService _transactionService;
        public BalanceController(IBlockchainService blockchainService, ITransactionService transactionService)
        {
            _blockchainServ = blockchainService;
            _transactionService = transactionService;
        }

        // [HttpGet("balance/{publicKey}")]
        [HttpPost("/")]
        public IActionResult Get(string publicKey)
        {
            if (publicKey == "") return BadRequest();
            List<Transaction> lstCurrentTransactions = _transactionService.GetAll();
            BalanceService balanceServ = new BalanceService(publicKey, lstCurrentTransactions, _blockchainServ.Get());
            decimal balance = balanceServ.Get();
            return Ok(balance);
        }
    }
}