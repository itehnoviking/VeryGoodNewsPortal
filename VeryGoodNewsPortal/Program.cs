using Microsoft.EntityFrameworkCore;
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
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DataBase
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<VeryGoodNewsPortalContext>(opt => opt.UseSqlServer(connectionString));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //DependencyInjection
            builder.Services.AddScoped<IRepository<Article>, ArticleRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IArticleServices, ArticleServices>();

            //AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}





