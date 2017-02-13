using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using XpressAccess.Identity.Manager;
using XpressAccess.Identity.EFStore;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to ServiceBuilder for adding entity framework stores.
    /// </summary>
    public static class EFBuilderExtensions
    {
        /// <summary>
        /// Adds an Entity Framework implementation of identity stores.
        /// </summary>
        /// <typeparam name="TContext">The Entity Framework database context to use.</typeparam>
        /// <param name="builder">The ServiceBuilder instance this method extends.</param>
        /// <returns>The ServiceBuilder instance this method extends.</returns>
        public static ServiceBuilder AddXAIdentityEFStores<TContext>(this ServiceBuilder builder)
            where TContext : DbContext
        {
            AddStores(builder._services, builder._userType, builder._roleType, builder._userRoleType, typeof(TContext));
            return builder;
        }

        private static void AddStores(IServiceCollection services, Type userType, Type roleType, Type userRoleType, Type contextType)
        {
            services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), typeof(UserStore<,,,>).MakeGenericType(userType, roleType, userRoleType, contextType));

            services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), typeof(RoleStore<,>).MakeGenericType(roleType, contextType));
        }
    }
}
