using System;
using Microsoft.Extensions.DependencyInjection;

namespace XpressAccess.Identity.Manager
{
    /// <summary>
    /// Configuring Xpress Access Identity store services.
    /// </summary>
    public class ServiceBuilder
    {
        public Type _userType { get; private set; }

        public Type _roleType { get; private set; }

        public Type _userRoleType { get; private set; }

        public IServiceCollection _services { get; private set; }

        /// <summary>
        /// Creates a new instance of ServiceBuilder.
        /// </summary>
        /// <param name="user">Type of user.</param>
        /// <param name="role">Type of role.</param>
        /// <param name="services">The IServiceCollection to attach to.</param>
        public ServiceBuilder(Type user, Type role, Type userrole, IServiceCollection services)
        {
            _userType = user;
            _roleType = role;
            _userRoleType = userrole;
            _services = services;
        }
    }
}
