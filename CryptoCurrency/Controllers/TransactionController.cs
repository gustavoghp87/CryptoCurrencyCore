using Models;
using Services.Interfaces;
using Services.Transactions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoCurrency.Controllers
{
    [ApiController]
    [Route("api/")]
    [EnableCors("MyCors")]
    [Produces("application/json")]
    public class TransactionController : ControllerBase
    {
        private Blockchain _blockchain;
        private IBlockchainService _blockchainServ;
        private ITransactionService _transactionService;
        private ISignTransactionService _signTransactionService;
        public TransactionController(IBlockchainService blockchainService, ITransactionService transactionService,
            SignTransactionService signTransactionService)
        {
            _blockchainServ = blockchainService;
            _blockchain = _blockchainServ.Get();
            _transactionService = transactionService;
            _signTransactionService = signTransactionService;
        }

        [HttpGet("transaction")]
        public IActionResult GetAll()
        {
            List<Transaction> lstTransactions = _transactionService.GetAll();
            return Ok(lstTransactions);
        }

        [HttpPost("transaction")]
        public async Task<IActionResult> AddTransaction(Transaction transactionRequest)
        {
            bool response = await _transactionService.Add(transactionRequest);
            if (!response) return BadRequest();
            return Ok(transactionRequest);
        }

        [HttpPost("transaction/signature")]
        public IActionResult Sign(Transaction transactionRequest, string privateKey)
        {
            if (privateKey == null | privateKey == "") return BadRequest();
            _signTransactionService.Initialize(transactionRequest, privateKey);
            transactionRequest.Signature = _signTransactionService.GetSignature();
            return Ok(transactionRequest);
        }
    }
}