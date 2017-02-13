using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XpressAccess.Identity.EFStore;
using XpressAccess.Identity.Manager;

namespace XpressAccess.Identity.Service
{
    /// <summary>
    /// Initialize IdentityServer Configuration data (resources and clients) and default user and role in database.
    /// </summary>
    public static class IdentityConfigInitializer
    {
        /// <summary>
        /// Initialize IdentityServer Configuration data (resources and clients) and default user and role in database.
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider to retrieve service.</param>
        public static async Task InitializeDbAsync(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                // add IdentityServer operation db
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                // add IdentityServer configuration data (Clients, IdentityResources, ApiResources
                var configContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                await configContext.Database.MigrateAsync();

                // add Clients
                if (!configContext.Clients.Any())
                {
                    foreach (var client in IdentityConfig.GetClients())
                    {
                        configContext.Clients.Add(client.ToEntity());
                    }
                    await configContext.SaveChangesAsync();
                }

                // add IdentityResources
                if (!configContext.IdentityResources.Any())
                {
                    foreach (var identityResource in IdentityConfig.GetIdentityResources())
                    {
                        configContext.IdentityResources.Add(identityResource.ToEntity());
                    }
                    await configContext.SaveChangesAsync();
                }

                // add ApiResources
                if (!configContext.ApiResources.Any())
                {
                    foreach (var apiResource in IdentityConfig.GetApiResources())
                    {
                        configContext.ApiResources.Add(apiResource.ToEntity());
                    }
                    await configContext.SaveChangesAsync();
                }

                // Add default identity user and roles
                var identityCntext = serviceScope.ServiceProvider.GetService<IdentityDbContext>();
                await identityCntext.Database.MigrateAsync();

                // create deafule roles
                if (!identityCntext.Roles.Any())
                {
                    var roleManager = serviceProvider.GetService<IdentityRoleMaanger<IdentityRole>>();

                    // create a few roles
                    IdentityRole role1 = new IdentityRole { Name = "PortalAdmin" };
                    IdentityRole role2 = new IdentityRole { Name = "ClientAdmin" };
                    IdentityRole role3 = new IdentityRole { Name = "ClientUser" };

                    await roleManager.CreateAsync(role1);
                    await roleManager.CreateAsync(role2);
                    await roleManager.CreateAsync(role3);
                }

                // create default Portal Administrator
                if (!identityCntext.Users.Any())
                {
                    var userManager = serviceProvider.GetService<IdentityUserManager<IdentityUser>>();

                    var user = await userManager.FindbyEmailAsync("admin@xpressaccess.com");
                    if (user == null)
                    {
                        user = new IdentityUser { Email = "admin@xpressaccess.com" };
                        await userManager.CreateAsync(user, "password");
                        await userManager.AssignToRoleAsync(user, "PortalAdmin");
                    }
                }
            }
        }
    }
}
