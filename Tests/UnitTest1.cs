using System;
using Xunit;
using CryptoCurrency.Services;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Issuer.Get();
        }

        [Fact]
        public void Test2()
        {
            var miner = Miner.Get();
            Console.WriteLine(miner.PublicKey);
            Assert.Equal(miner.PrivateKey, "L4fkiGDz1jdeTqo2rDUehWEWtDi3zhTnHwETi46zN9XGLoiAb9Rd");
        }
    }
}
