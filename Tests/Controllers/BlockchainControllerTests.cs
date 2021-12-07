using CryptoCurrency.Controllers;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using Services.Interfaces;
using System.Net;
using Xunit;

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
        public void Mine_When_True()
        {
            // arrange
            Blockchain blockchain = new();
            _blockchainServiceMock
                .Setup(x => x.Get())
                .Returns(() => blockchain)
            ;
            _blockchainServiceMock
                .Setup(x => x.Mine())
                .Returns(() => true)
            ;

            // act
            IActionResult actionResult = _controller.Mine();
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
        public void Mine_When_False()
        {
            // arrange
            _blockchainServiceMock
                .Setup(x => x.Mine())
                .Returns(() => false)
            ;

            // act
            IActionResult actionResult = _controller.Mine();
            //var statusCode = (HttpStatusCode)actionResult
            //    .GetType()
            //    .GetProperty("StatusCode")
            //    .GetValue(actionResult, null);

            // assert
            //Assert.Equal("BadRequest", statusCode.ToString());
        }
    }
}