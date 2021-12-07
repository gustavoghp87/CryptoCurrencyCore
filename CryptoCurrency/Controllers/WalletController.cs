using CryptoCurrency.Controllers.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using Services.Wallets;
using System.Collections.Generic;

namespace CryptoCurrency.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("MyCors")]
    [Produces("application/json")]
    public class WalletController : ControllerBase, IWalletController
    {
        private readonly ITransactionService _transactionService;
        private readonly IBlockchainService _blockchainService;
        public WalletController(ITransactionService transactionService, IBlockchainService blockchainService)
        {
            _transactionService = transactionService;
            _blockchainService = blockchainService;
        }
        [HttpGet]
        public IActionResult GetNew()
        {
            Wallet wallet = WalletService.Generate();
            return Ok(wallet);
        }

        [HttpGet("{publicKey}")]
        //[HttpPost]
        public IActionResult GetBalance(string publicKey)
        {
            if (string.IsNullOrEmpty(publicKey)) return BadRequest();
            decimal balance = WalletService.GetBalance(publicKey, _blockchainService.Get(), _transactionService.GetAll());
            return Ok(balance);
        }

        [HttpGet("transactions/{publicKey}")]
        //[HttpPost]
        public IActionResult GetTransactions(string publicKey)
        {
            if (string.IsNullOrEmpty(publicKey)) return BadRequest();
            List<Transaction> transactionsList =
                WalletService.GetTransactionsByAddress(publicKey, _blockchainService.Get(), _transactionService.GetAll());
            return Ok(transactionsList);
        }

        [HttpGet("miner")]
        public IActionResult GetMinerBalance()
        {
            decimal balance =
                WalletService.GetBalance(Miner.Wallet.PublicKey, _blockchainService.Get(), _transactionService.GetAll());
            return Ok(balance);
        }

        [HttpGet("issuer")]
        public IActionResult GetIssuerBalance()
        {
            decimal balance =
                WalletService.GetBalance(Issuer.Wallet.PublicKey, _blockchainService.Get(), _transactionService.GetAll());
            return Ok(balance);
        }
    }
}