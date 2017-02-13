using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using XpressAccess.Identity.EFStore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace XpressAccess.Portal
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // configure db
            var sqlConnectionString = Configuration.GetConnectionString("XADbConnection");
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(sqlConnectionString));

            // Configure XA Identity Manager
            services.AddXAIdentityManager<IdentityUser, IdentityRole, IdentityUserRole>()
                .AddXAIdentityEFStores<IdentityDbContext>();

            // Configure authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Portal Admin", p =>
                {
                    p.RequireClaim("role", "PortalAdmin");
                });
                options.AddPolicy("Client Admin", p =>
                {
                    p.RequireClaim("role", new[] { "PortalAdmin", "ClientAdmin" });
                });

            });

            // configure access policy - this will trigger the authentication handshake with IdentityServer
            // disable this to show interactive login
            /*var accessPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("scope", "XAPortal")
                .Build();

            // Add framework services.
            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter(accessPolicy));
            });*/

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "cookie",
                // use this to handle access denied return from IdentityServer
                AccessDeniedPath = "/Test/Forbidden"
            });

            // turn off default mapper
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                AuthenticationScheme = "oidc",
                SignInScheme = "cookie",
                Authority = "http://localhost:5000",
                RequireHttpsMetadata = env.IsDevelopment() ? false : true,
                PostLogoutRedirectUri = "http://localhost:5002",
                
                ClientId = "mvcPortal",
                ClientSecret = "mvcSecret",
                ResponseType = "code id_token",
                Scope = { "portalscope" },
                GetClaimsFromUserInfoEndpoint = true,
                SaveTokens = true,
                // this is to support legacy name, role mapping because the default mapper is turned off
                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role",
                }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
