using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using XpressAccess.Identity.EFStore;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.Validation;
using IdentityServer4.Services;
using IdentityServer4.EntityFramework;
using System.Reflection;

namespace XpressAccess.Identity.Service
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // configure db
            var sqlConnectionString = Configuration.GetConnectionString("XADbConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(sqlConnectionString, m => m.MigrationsAssembly(migrationsAssembly)));

            // Configure XA Identity Manager
            services.AddXAIdentityManager<IdentityUser, IdentityRole, IdentityUserRole>()
                .AddXAIdentityEFStores<IdentityDbContext>();

            // configure IdentityServer
            services.AddIdentityServer()
                .AddTemporarySigningCredential()
                .AddConfigurationStore(builder => builder.UseSqlServer(sqlConnectionString, m => m.MigrationsAssembly(migrationsAssembly)))
                .AddOperationalStore(builder => builder.UseSqlServer(sqlConnectionString, m => m.MigrationsAssembly(migrationsAssembly)));

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>()
                .AddTransient<IProfileService, ProfileService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            // add IdentityServer
            app.UseIdentityServer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            // Initialize identity db
            IdentityConfigInitializer.InitializeDbAsync(app.ApplicationServices).Wait();
        }
    }
}
