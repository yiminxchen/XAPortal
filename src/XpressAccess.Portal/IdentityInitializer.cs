using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XpressAccess.Identity.EFStore;
using XpressAccess.Identity.Manager;

namespace XpressAccess.Portal
{
    /// <summary>
    /// Initialize Identity database
    /// </summary>
    public static class IdentityInitializer
    {
        /// <summary>
        /// Add default identity user(admin@xpressaccess.com) to identity store.
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider to retrieve service.</param>
        public static async Task InitializeIdentityStoreAsync(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbCntext = serviceScope.ServiceProvider.GetService<IdentityDbContext>();

                if (await dbCntext.Database.EnsureCreatedAsync())
                {
                    var roleManager = serviceProvider.GetService<IdentityRoleMaanger<IdentityRole>>();

                    // create a few roles
                    IdentityRole role1 = new IdentityRole { Name = "superuser" };
                    IdentityRole role2 = new IdentityRole { Name = "ClientAdmin" };
                    IdentityRole role3 = new IdentityRole { Name = "ClientUser" };

                    await roleManager.CreateAsync(role1);
                    await roleManager.CreateAsync(role2);
                    await roleManager.CreateAsync(role3);

                    // create default admin
                    var userManager = serviceProvider.GetService<IdentityUserManager<IdentityUser>>();

                    var user = await userManager.FindbyEmailAsync("admin@xpressaccess.com");
                    if (user == null)
                    {
                        user = new IdentityUser { Email = "admin@xpressaccess.com" };
                        await userManager.CreateAsync(user, "password");
                        await userManager.AssignToRoleAsync(user, "superuser");
                    }
                }
            }
        }
    }
}
