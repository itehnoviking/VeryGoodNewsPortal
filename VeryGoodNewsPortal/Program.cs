using FirstMvcApp.Core.SerilogSinks;
using FirstMvcApp.Domain.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Diagnostics;
using VeryGoodNewsPortal.Core.Data;
using VeryGoodNewsPortal.Core.Interfaces;
using VeryGoodNewsPortal.Core.Interfaces.Data;
using VeryGoodNewsPortal.Data;
using VeryGoodNewsPortal.Data.Entities;
using VeryGoodNewsPortal.DataAccess;
using VeryGoodNewsPortal.Domain.Services;

namespace VeryGoodNewsPortal
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .UseSerilog((ctx, lc) =>
               {
                   lc.MinimumLevel.Information().WriteTo.Console();
                   lc.MinimumLevel.Debug().WriteTo.CustomSink();
                   lc.MinimumLevel.Warning().WriteTo.File(@"C:\Users\itehn\OneDrive\Рабочий стол\log.log");
               }).ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });

        public static async Task Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

        }
    }
}





