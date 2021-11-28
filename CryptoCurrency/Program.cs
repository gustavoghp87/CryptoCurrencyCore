using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Services.Blockchains;
using Services.Interfaces;
using Services.Nodes;
using Services.Transactions;

namespace CryptoCurrency
{
    public class Program
    {
        //internal static readonly string AppUrl = "http://localhost:5001";

        public static void Main(string[] args)
        {
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
