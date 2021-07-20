using System;
using Xunit;
using Models;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }

        [Theory]
        [InlineData("one")]
        [InlineData("two")]
        [InlineData("three")]
        public void Test2(string some)
        {
            Console.WriteLine(some);
            var miner = Miner.MinerWallet;
            Console.WriteLine(miner.PublicKey);
            Assert.Equal("L4fkiGDz1jdeTqo2rDUehWEWtDi3zhTnHwETi46zN9XGLoiAb9Rd", miner.PrivateKey);
        }
    }
}
