using Models;
using Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Services;
using CryptoCurrency.Controllers.Interfaces;

namespace CryptoCurrency.Controllers
{
    [ApiController]
    [Route("api/")]
    [EnableCors("MyCors")]
    [Produces("application/json")]
    public class WalletController : ControllerBase, IWalletController
    {  
        private IBlockchainService _blockchainServ;
        private ITransactionService _transactionService;
        private IBalanceService _balanceServ;
        public WalletController(IBlockchainService blockchainService, ITransactionService transactionService,
            IBalanceService balanceServ)
        {
            _blockchainServ = blockchainService;
            _transactionService = transactionService;
            _balanceServ = balanceServ;
        }

        [HttpGet("/")]
        public IActionResult GetNew()
        {
            Wallet wallet = WalletService.Generate();
            return Ok(wallet);
        }

        // [HttpGet("balance/{publicKey}")]
        [HttpPost("/")]
        public IActionResult GetBalance(string publicKey)
        {
            if (publicKey == "") return BadRequest();
            List<Transaction> lstCurrentTransactions = _transactionService.GetAll();
            _balanceServ.Initialize(publicKey, lstCurrentTransactions, _blockchainServ.Get());
            decimal balance = _balanceServ.Get();
            return Ok(balance);
        }
    }
}