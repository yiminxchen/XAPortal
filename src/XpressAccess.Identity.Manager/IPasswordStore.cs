using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XpressAccess.Identity.Manager
{
    /// <summary>
    /// Provides an abstraction for password management of identity users.
    /// </summary>
    /// <typeparam name="TUser">The type that represents an identity user.</typeparam>
    public interface IPasswordStore<TUser> where TUser : class
    {
        /// <summary>
        /// Set passsword for identity user in the user store.
        /// </summary>
        /// <param name="user">The identity user.</param>
        /// <returns>An asynchronous Task.</returns>
        Task SetPasswordAsync(TUser user, string password);

        /// <summary>
        /// Set passsword for identity user in the user store.
        /// </summary>
        /// <param name="user">The identity user.</param>
        /// <returns>An asynchronous Task.</returns>
        Task<string> GetPasswordAsync(TUser user);
    }
}
