using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
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
            Console.WriteLine(DateTime.Now);
            Console.WriteLine("Local: " + DateTime.UtcNow);
            Console.WriteLine("Starting app with domain name " + DomainName);
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
