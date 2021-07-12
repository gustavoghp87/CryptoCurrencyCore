using System.Net;
using System;
using Xunit;
using CryptoCurrency.Services;
using CryptoCurrency.Controllers;
using Moq;
using CryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Controllers
{
    public class Blockchain : ControllerBase
    {
        private Mock<BlockchainController> _controller;
        private Mock<IBlockchainService> _blockchainServiceMock = new Mock<IBlockchainService>();
        public Blockchain()
        {
            _controller = new Mock<BlockchainController>(_blockchainServiceMock);
        }

        [Fact]
        public void Test1()
        {
            CryptoCurrency.Models.Blockchain blockchain = new() {
                LastReward = 50,
                LastDifficulty = "6 zeros"
            };
            // var resp = _controller
            //     .Setup(x => x.Mine())
            //     .Returns<IActionResult>(Ok);

            //var actionResult = (ViewResult) _controller.Get();
        }
    }
}