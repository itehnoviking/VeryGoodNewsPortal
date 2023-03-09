using FirstMvcApp.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using VeryGoodNewsPortal.Core.Data;
using VeryGoodNewsPortal.Core.Interfaces;
using VeryGoodNewsPortal.Core.Interfaces.Data;
using VeryGoodNewsPortal.Core.Interfaces.InterfacesCqs;
using VeryGoodNewsPortal.Data;
using VeryGoodNewsPortal.Data.Entities;
using VeryGoodNewsPortal.DataAccess;
using VeryGoodNewsPortal.Domain.Services;
using VeryGoodNewsPortal.Domain.ServicesCqs;

namespace VeryGoodNewsPortal.WebApi
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

            services.AddScoped<IPositivityGradeService, PositivityGradeService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ISourceService, SourceService>();
            services.AddScoped<IRssService, RssService>();
            services.AddScoped<IHtmlParserService, HtmlParserService>();

            services.AddScoped<IArticleServiceCqs, ArticleServiceCqs>();

            //AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add Hangfire services.
            //services.AddHangfire(configuration => configuration
            //    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            //    .UseSimpleAssemblyNameTypeSerializer()
            //    .UseRecommendedSerializerSettings()
            //    .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
            //    {
            //        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //        QueuePollInterval = TimeSpan.Zero,
            //        UseRecommendedIsolationLevel = true,
            //        DisableGlobalLocks = true
            //    }));

            //// Add the processing server as IHostedService
            //services.AddHangfireServer();

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(options =>
            //    {
            //        options.SaveToken = true;
            //        options.RequireHttpsMetadata = true;
            //        options.TokenValidationParameters = new TokenValidationParameters()
            //        {
            //            ValidateIssuerSigningKey = true,
            //            ValidateIssuer = false,
            //            ValidateAudience = false,
            //            ClockSkew = TimeSpan.Zero,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:Secret"]))
            //        };
            //    });

            Assembly.Load("VeryGoodNewsPortal.Cqs");
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiFirstMvcApp", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiFirstMvcApp v1"));
            }

            //app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
