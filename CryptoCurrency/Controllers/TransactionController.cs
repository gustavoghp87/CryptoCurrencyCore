using CryptoCurrency.Controllers.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using Services.Transactions;
using System.Collections.Generic;

namespace CryptoCurrency.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("MyCors")]
    [Produces("application/json")]
    public class TransactionController : ControllerBase, ITransactionController
    {
        private ITransactionService _transactionService;
        private IBlockchainService _blockchainService;
        public TransactionController(ITransactionService transactionService, IBlockchainService blockchainService)
        {
            _transactionService = transactionService;
            _blockchainService = blockchainService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Transaction> lstTransactions = _transactionService.GetAll();
            return Ok(lstTransactions);
        }

        [HttpPost]
        public IActionResult AddTransaction(Transaction transactionRequest)
        {
            bool response = _transactionService.Add(transactionRequest, _blockchainService.Get());
            if (!response) return BadRequest();
            return Ok(transactionRequest);
        }

        [HttpPatch]
        public IActionResult Sign(Transaction transactionRequest, string privateKey)
        {
            if (privateKey == null | privateKey == "") return BadRequest();    // TODO: validation
            transactionRequest.Signature = TransactionSignature.Sign(transactionRequest, privateKey);
            transactionRequest.Miner = Miner.Wallet.PublicKey;
            return Ok(transactionRequest);
        }
    }
}