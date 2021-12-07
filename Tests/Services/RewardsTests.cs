using System.Net;
using Xunit;
using CryptoCurrency.Controllers;
using Moq;
using Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Services.Blockchains;
using Models;

namespace Tests.Services
{
    public class RewardsTests : ControllerBase
    {
        private readonly Mock<IBlockchainService> _blockchainServiceMock;
        private readonly BlockchainController _controller;
        public RewardsTests()
        {
            _blockchainServiceMock = new Mock<IBlockchainService>();
            _controller = new BlockchainController(_blockchainServiceMock.Object);
        }

        [Fact]
        public void CalculateRewards()
        {
            //decimal result0 = Reward.Get(0);
            //decimal result1 = Reward.Get(-1);
            decimal result2 = Reward.Get(99);
            decimal result3 = Reward.Get(100);
            decimal result4 = Reward.Get(101);
            decimal result5 = Reward.Get(199);
            decimal result6 = Reward.Get(200);
            decimal result7 = Reward.Get(201);
            decimal result8 = Reward.Get(500);
            decimal result9 = Reward.Get(999);
            decimal result10 = Reward.Get(1000);
            decimal result11 = Reward.Get(1001);
            decimal result12 = Reward.Get(1200);
            decimal result13 = Reward.Get(1201);

            //Assert.(result0, 50);
            //Assert.Equal(result1, 50);
            //Assert.Equal(result2, 50);
            //Assert.Equal(result3, 50);
            //Assert.Equal(result4, 25);
            //Assert.Equal(result5, 25);
            //Assert.Equal(result6, 25);
            //Assert.Equal(result7, (decimal)12.5);
            //Assert.Equal(result8, (decimal)3.125);
            //Assert.Equal(result9, (decimal)0.00000005);
            //Assert.Equal(result10, (decimal)0.00000005);
            //Assert.Equal(result11, (decimal)0.000000025);
            //Assert.Equal(result12, 0);
            //Assert.Equal(result13, 0);
        }
    }
}