using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XpressAccess.Identity.Manager
{
    /// <summary>
    /// Provide APIs for managing identity role in a persistent store.
    /// </summary>
    /// <typeparam name="TRole">The type of the role.</typeparam>
    public class IdentityRoleMaanger<TRole> where TRole : class
    {
        /// <summary>
        /// Gets or sets the persistent store for IdentityRoleManager to work with.
        /// </summary>
        protected internal IRoleStore<TRole> _store { get; private set; }

        /// <summary>
        /// Constructs a new instance of IdentityRoleMaanger.
        /// </summary>
        /// <param name="store">The persistent store for IdentityUserManaer to work with.</param>
        public IdentityRoleMaanger(IRoleStore<TRole> store)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }
            _store = store;
        }

        /// <summary>
        /// Creates the identity role in the persistent store
        /// </summary>
        /// <param name="role">The role to create.</param>
        /// <returns>An asynchronous Task, returns true if create operation is successful, otherwise returns false.
        /// </returns>
        public virtual async Task<bool> CreateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return await _store.CreateAsync(role);
        }
    }
}
