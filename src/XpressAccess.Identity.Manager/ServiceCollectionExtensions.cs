using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XpressAccess.Identity.Manager;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the Xpress Access Identity store configuration for the  User and Role Management.
        /// </summary>
        /// <typeparam name="TUser">The type of user.</typeparam>
        /// <typeparam name="TRole">The type of role.</typeparam>
        /// <typeparam name="TUserRole">The type of userrole.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <returns>An ServiceBuilder for creating and configuring the identity store.</returns>
        public static ServiceBuilder AddXAIdentityManager<TUser, TRole, TUserRole>(
            this IServiceCollection services)
            where TUser : class
            where TRole : class
            where TUserRole : class
        {
            services.TryAddScoped<IdentityUserManager<TUser>, IdentityUserManager<TUser>>();
            services.TryAddScoped<IdentityRoleMaanger<TRole>, IdentityRoleMaanger<TRole>>();

            return new ServiceBuilder(typeof(TUser), typeof(TRole), typeof(TUserRole), services);
        }
    }
}
