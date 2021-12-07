using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Services.Blockchains;
using Services.Interfaces;

namespace CryptoCurrency
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public static readonly string MyCors = "MyCors";
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptoCurrency", Version = "v2" });
            });
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                //options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                //options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            services.AddCors(o => o.AddPolicy(MyCors, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            //services.AddSingleton<IBlockchainService, BlockchainService>();
            //services.AddSingleton<ITransactionService, TransactionService>();
            //services.AddSingleton<INodeService, NodeService>();
            //services.AddTransient<IBalanceService, BalanceService>();
            //services.AddTransient<ISignTransactionService, SignTransactionService>();
            services.AddHostedService<MineHostedService>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ProgramModule());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBlockchainService blockchainService)
        {
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoCurrency v1"));
            //}
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(MyCors);
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            blockchainService.Get();
        }
    }
}
