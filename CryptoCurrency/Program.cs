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
        //private static readonly log4net.ILog log =
        //    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string DomainName { get =>
            Environment.GetEnvironmentVariable("domainName") ?? "https://localhost:5001/";
        }
        public static void Main(string[] args)
        {
            //log.Info("Hello logging world! " + DateTime.Now + ", local: " + DateTime.UtcNow);
            Console.WriteLine(DateTime.Now + ", local: " + DateTime.UtcNow);
            Console.WriteLine("Starting app with domain name " + DomainName);
            Issuer.Wallet = new() {
                PublicKey = "1GPuEJZ6rjh7WfdQwNqUPWgsud95RLBUfK",
                PrivateKey = "L27gRq59TSnXTWanV1SdgHRucFtfqZciec5Grooc6MDPe4o47T5V",
                BitcoinAddress = "12EWT461aNMMfjEGteJ6Bz8BWmDeB1Efkj"
            };
            MinerWallet.Load();
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
