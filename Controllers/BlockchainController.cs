using BlockchainAPI.Models;
using BlockchainAPI.Services.Blockchains;
using BlockchainAPI.Services.Transactions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAPI.Api
{
    [ApiController]
    // [Route("[controller]")]
    [EnableCors("MyCors")]
    [Produces("application/json")]
    [Route("/")]
    public class BlockchainController : ControllerBase
    {
        private Blockchain _blockchain;
        private IBlockchainService _blockchainServ;

        public BlockchainController(IBlockchainService blockchainService)
        {
            _blockchainServ = blockchainService;
            _blockchain = new();
            _blockchain = _blockchainServ.Get();
        }
        private void UpdateBlockchain()
        {
            _blockchain = _blockchainServ.Get();
        }

        [HttpGet()]
        public IActionResult GetBlockchain()
        {
            UpdateBlockchain();
            return Ok(_blockchain);
        }

        [HttpGet("/transaction")]
        public IActionResult GetTransactions()
        {
            List<Transaction> lstTransactions = _blockchainServ.GetTransactionService().GetAll();
            return Ok(lstTransactions);
        }

        [HttpPost("/transaction")]
        public async Task<IActionResult> AddTransaction(Transaction transactionRequest)
        {
            bool response = await _blockchainServ.GetTransactionService().Add(transactionRequest);
            if (!response) return BadRequest();
            UpdateBlockchain();
            return Ok(transactionRequest);
        }

        [HttpPost("/signature")]
        public IActionResult Sign(Transaction transactionRequest, string privateKey)
        {
            if (privateKey == null | privateKey == "") return BadRequest();
            var serv = new SignTransactionService(transactionRequest, privateKey);
            transactionRequest.Signature = serv.GetSignature();
            return Ok(transactionRequest);
        }

        [HttpGet("/mine")]
        public async Task<IActionResult> Mine()
        {
            bool response = await _blockchainServ.Mine();
            if (!response) return BadRequest();
            UpdateBlockchain();
            return Ok(_blockchain);
        }

        [HttpGet("/balance/{publicKey}")]
        public IActionResult GetBalance(string publicKey)
        {
            UpdateBlockchain();
            if (publicKey == "") return BadRequest();
            List<Transaction> lstCurrentTransactions = _blockchainServ.GetTransactionService().GetAll();
            BalanceService balanceServ = new BalanceService(publicKey, lstCurrentTransactions, _blockchain);
            decimal balance = balanceServ.Get();
            return Ok(balance);
        }

        [HttpGet("/registry")]
        public async Task<IActionResult> Register()
        {
            var httpClient = new HttpClient();
            var stringPayload = JsonConvert.SerializeObject(new RegistryPackage());
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            Console.WriteLine(httpContent);
            var httpResponse = await httpClient.PostAsync("http://localhost:10000/registry", httpContent);
            if (!httpResponse.IsSuccessStatusCode) return BadRequest();
            return Ok("Register");
            

            //if (httpResponse.Content != null)
            //{
            //    var responseContent = await httpResponse.Content.ReadAsStringAsync();
            //    Console.WriteLine(responseContent);
            //}

        }

        [HttpGet("/validation")]
        public IActionResult ValidateBlockchain()
        {
            UpdateBlockchain();
            bool response = ValidateBlockchainService.IsValid(_blockchain);
            return Ok(response);
        }

        public class RegistryPackage
        {
            public string Ip = "http://localhost:5000";
        }
    }
}
