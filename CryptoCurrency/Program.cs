using Autofac.Extensions.DependencyInjection;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Models;
using Services.Blockchains;
using System;
using System.IO;
using System.Reflection;

namespace CryptoCurrency
{
    public class Program
    {
        internal static string DomainName { get => Environment.GetEnvironmentVariable("domainName") ?? "https://localhost:5001/"; }
        public static readonly ILog DataLog = LogManager.GetLogger("first");
        public static readonly ILog ExceptionsLogs = LogManager.GetLogger("second");
        private static void ConfigureLog4net()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));    // Debug Info Warn Error Fatal
            GlobalContext.Properties["host"] = Environment.MachineName;
        }
        public static void Main(string[] args)
        {
            ConfigureLog4net();
            DataLog.Info("Starting app with domain name " + DomainName + " on " + DateTime.UtcNow + ", local: " + DateTime.Now);
            ExceptionsLogs.Warn("Second Starting app with domain name " + DomainName + " on " + DateTime.UtcNow + ", local: " + DateTime.Now);
            Issuer.Wallet = new() {
                PublicKey = "1GPuEJZ6rjh7WfdQwNqUPWgsud95RLBUfK",
                PrivateKey = "L27gRq59TSnXTWanV1SdgHRucFtfqZciec5Grooc6MDPe4o47T5V",
                BitcoinAddress = "12EWT461aNMMfjEGteJ6Bz8BWmDeB1Efkj"
            };
            MinerWallet.Load();
            BlockchainService.DomainName = DomainName;
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                ExceptionsLogs.Fatal(DateTime.UtcNow + " - " + e.Message);
                throw;
            }
        }
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.UseUrls(AppUrl);
                });
    }
}
