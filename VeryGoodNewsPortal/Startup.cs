using FirstMvcApp.Domain.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddInMemoryCollection();

            Configuration = configurationBuilder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            //var z = Configuration["isDebugMode"];
            //var y = Configuration["data"];
            //var x = Configuration["TEMP"];

            services.AddDbContext<VeryGoodNewsPortalContext>(opt
            => opt.UseSqlServer(connectionString));

            //DependencyInjection
            services.AddScoped<IRepository<Article>, ArticleRepository>();
            services.AddScoped<IRepository<Comment>, CommentsRepository>();
            services.AddScoped<IRepository<Role>, RoleRepository>();
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<Source>, SourceRepository>();
            services.AddScoped<IRepository<UserRole>, UserRoleRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<IRssService, RssService>();
            services.AddScoped<IHtmlParserService, HtmlParserService>();

            //AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            //services
            //.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //.AddCookie(opt =>
            //{
            //    opt.LoginPath = "/account/login";
            //    opt.AccessDeniedPath = "/access-denied";
            //});

            services.AddAuthorization();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseHangfireDashboard();

            //var rssService = serviceProvider.GetRequiredService<IRssService>();

            var rssService = serviceProvider.GetRequiredService<IRssService>();
            RecurringJob.AddOrUpdate("Aggregation articles from rss",
                () => rssService.AggregateArticleDataFromRssSources(),
                Cron.Hourly());
        }
    }
}
