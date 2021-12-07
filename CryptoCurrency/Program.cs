using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Models;
using Services.Blockchains;
using System;

namespace CryptoCurrency
{
    public class Program
    {
        public static string DomainName { get =>
            Environment.GetEnvironmentVariable("domainName") ?? "https://localhost:5001/";
        }
        public static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now + ", local: " + DateTime.UtcNow);
            Console.WriteLine("Starting app with domain name " + DomainName);
            Miner.Wallet = new();
            Miner.Wallet.PublicKey = "1BPiqwmT9ig8cSfeRCiJaJU7qK4KrPKWhc";
            Miner.Wallet.PrivateKey = "L4fkiGDz1jdeTqo2rDUehWEWtDi3zhTnHwETi46zN9XGLoiAb9Rd";
            Miner.Wallet.BitcoinAddress = "1CvAdfEfhfhGSF8kbK7r2sB4DcKcSQi8GT";
            Issuer.Wallet = new();
            Issuer.Wallet.PublicKey = "1GPuEJZ6rjh7WfdQwNqUPWgsud95RLBUfK";
            Issuer.Wallet.PrivateKey = "L27gRq59TSnXTWanV1SdgHRucFtfqZciec5Grooc6MDPe4o47T5V";
            Issuer.Wallet.BitcoinAddress = "12EWT461aNMMfjEGteJ6Bz8BWmDeB1Efkj";
            BlockchainService.DomainName = DomainName;
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.UseUrls(AppUrl);
                });
    }
}
