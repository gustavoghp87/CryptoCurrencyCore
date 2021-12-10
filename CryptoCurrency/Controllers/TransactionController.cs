using CryptoCurrency.Controllers.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using Services.Transactions;
using System;
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
        public IActionResult AddTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid data");
            bool response = _transactionService.Add(transaction, _blockchainService.Get());
            if (!response) return BadRequest();
            return Ok(transaction);
        }

        public class PrivateKey
        {
            public string Key;
        }

        [HttpPatch]
        public IActionResult Sign(TransactionRequest transactionRequest)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid data");
            if (transactionRequest == null || transactionRequest.PrivateKey == null || transactionRequest.PrivateKey == "")
                return BadRequest("Invalid data 2");    // TODO: validation
            Transaction transaction = new()
            {
                Amount = transactionRequest.Amount,
                Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(transactionRequest.Timestamp),
                Fees = transactionRequest.Fees,
                Miner = Miner.Wallet.PublicKey,
                Recipient = transactionRequest.Recipient,
                Sender = transactionRequest.Sender,
                Timestamp = transactionRequest.Timestamp
            };
            transaction.Signature = TransactionSignature.Sign(transaction, transactionRequest.PrivateKey);
            if (string.IsNullOrEmpty(transaction.Signature)) return BadRequest("Invalid key");
            return Ok(transaction);
        }
    }
}