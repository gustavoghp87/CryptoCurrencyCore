using cryptoCurrency.Models;

namespace cryptoCurrency.Services
{
    public static class Miner
    {
        public static Wallet Get()
        {
            return new Wallet
            {
                PublicKey = "1BPiqwmT9ig8cSfeRCiJaJU7qK4KrPKWhc",
                PrivateKey = "L4fkiGDz1jdeTqo2rDUehWEWtDi3zhTnHwETi46zN9XGLoiAb9Rd",
                BitcoinAddress = "1CvAdfEfhfhGSF8kbK7r2sB4DcKcSQi8GT"
            };
        }
    }
}
