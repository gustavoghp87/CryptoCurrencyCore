using System.Net;
using Xunit;
using CryptoCurrency.Controllers;
using Moq;
using CryptoCurrency.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CryptoCurrency.Models;

namespace Tests.Controllers
{
    public class BlockchainControllerTests : ControllerBase
    {
        private readonly Mock<IBlockchainService> _blockchainServiceMock;
        private readonly BlockchainController _controller;
        public BlockchainControllerTests()
        {
            _blockchainServiceMock = new Mock<IBlockchainService>();
            _controller = new BlockchainController(_blockchainServiceMock.Object);
        }

        [Fact]
        public async void Mine_When_True()
        {
            // arrange
            Blockchain blockchain = new();
            _blockchainServiceMock
                .Setup(x => x.Get())
                .Returns(() => blockchain)
            ;
            _blockchainServiceMock
                .Setup(x => x.Mine())
                .ReturnsAsync(() => true)
            ;

            // act
            IActionResult actionResult = await _controller.Mine();
            var statusCode = (HttpStatusCode)actionResult
                .GetType()
                .GetProperty("StatusCode")
                .GetValue(actionResult, null)
            ;
            var objectAttached = actionResult
                .GetType()
                .GetProperty("Value")
                .GetValue(actionResult, null)
            ;

            // assert
            Assert.Equal("OK", statusCode.ToString());
            Assert.IsType<Blockchain>(objectAttached);
        }

        [Fact]
        public async void Mine_When_False()
        {
            // arrange
            _blockchainServiceMock
                .Setup(x => x.Mine())
                .ReturnsAsync(() => false)
            ;

            // act
            IActionResult actionResult = await _controller.Mine();
            var statusCode = (HttpStatusCode)actionResult
                .GetType()
                .GetProperty("StatusCode")
                .GetValue(actionResult, null);

            // assert
            Assert.Equal("BadRequest", statusCode.ToString());
        }
    }
}