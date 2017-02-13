using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XpressAccess.Identity.Manager
{
    /// <summary>
    /// Provide APIs for managing identity user in a persistent store.
    /// </summary>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    public class IdentityUserManager<TUser> where TUser : class
    {
        /// <summary>
        /// Gets or sets the persistent store for IdentityUserManager to work with.
        /// </summary>
        protected internal IUserStore<TUser> _store { get; private set; }

        /// <summary>
        /// Constructs a new instance of IdentityUserManager.
        /// </summary>
        /// <param name="store">The persistent store for IdentityUserManaer to work with.</param>
        public IdentityUserManager(IUserStore<TUser> store)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }
            _store = store;
        }

        /// <summary>
        /// Creates the identity user in the persistent store
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>An asynchronous Task, returns true if create operation is successful, otherwise returns false.
        /// </returns>
        public virtual async Task<bool> CreateAsync(TUser user, string password)
        {
            // password needs to be well protected, this is only for POC
            var passwordStore = GetPasswordStore();
            await UpdatePassword(passwordStore, user, password);
            return await _store.CreateAsync(user);
        }

        /// <summary>
        /// Find identity user that matches the email.
        /// </summary>
        /// <param name="email">The email to search for.</param>
        /// <returns>
        /// An asynchronous operation, returns identity user if found
        /// </returns>
        public virtual Task<TUser>FindbyEmailAsync(string email)
        {
            return email == null ? Task.FromResult<TUser>(null) : _store.FindByEmailAsync(email);
        }

        /// <summary>
        /// Find identity user that matches the Id.
        /// </summary>
        /// <param name="Id">The Id to search for.</param>
        /// <returns>
        /// An asynchronous operation, returns identity user if found
        /// </returns>
        public virtual Task<TUser> FindbyIdAsync(string Id)
        {
            return Id == null ? Task.FromResult<TUser>(null) : _store.FindByIdAsync(Guid.Parse(Id));
        }

        /// <summary>
        /// Validate identity user using email and password.
        /// </summary>
        /// <param name="email">The email of the identity user to validate.</param>
        /// <param name="password">The password of the identity user to validate.</param>
        /// <returns>An asynchronous Task returns true if the identity is validated, otherwise returns false.</returns>
        public virtual async Task<bool> ValidateIdentityAsync(string email, string password)
        {
            return  await _store.ValidateIdentityAsync(email, password);
        }

        /// <summary>
        /// Assign role to user.
        /// </summary>
        /// <param name="user">The identity user to add the identity role to.</param>
        /// <param name="roleName">The identity role name to add.</param>
        /// <returns>An asynchronous Task</returns>
        public virtual async Task AssignToRoleAsync(TUser user, string roleName)
        {
            var userRoleStore = GetUserRoleStore();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (await userRoleStore.IsInRoleAsync(user, roleName))
            {
                return;
            }
            await userRoleStore.AssignToRoleAsync(user, roleName);
            await _store.UpdateAsync(user);
        }

        /// <summary>
        /// Get all roles that the user belongs to
        /// </summary>
        /// <param name="user">The identity user to get roles.</param>
        /// <returns>An asynchronous Task returns a list of identity role for the identity user.</returns>
        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            var userRoleStore = GetUserRoleStore();
            return user == null ? Task.FromResult<IList<string>>(new List<string>()) : userRoleStore.GetRolesAsync(user);
        }

        internal async Task UpdatePassword(IPasswordStore<TUser> passwordStore, TUser user, string newPassword)
        {
            await passwordStore.SetPasswordAsync(user, newPassword);
            return;
        }

        private IPasswordStore<TUser> GetPasswordStore()
        {
            var cast = _store as IPasswordStore<TUser>;
            if (cast == null)
            {
                throw new NotImplementedException("Password store is not implemented");
            }
            return cast;
        }

        private IUserRoleStore<TUser> GetUserRoleStore()
        {
            var cast = _store as IUserRoleStore<TUser>;
            if (cast == null)
            {
                throw new NotImplementedException("Password store is not implemented");
            }
            return cast;
        }
    }
}
