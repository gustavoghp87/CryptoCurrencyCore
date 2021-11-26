using CryptoCurrency.Controllers.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using Services.Interfaces;
using System.Collections.Generic;

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

        [HttpGet("/wallet")]
        public IActionResult GetNew()
        {
            Wallet wallet = WalletService.GenerateWallet();
            return Ok(wallet);
        }

        // [HttpGet("balance/{publicKey}")]
        [HttpPost("/wallet")]
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