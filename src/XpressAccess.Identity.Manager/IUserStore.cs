using System;
using System.Threading.Tasks;

namespace XpressAccess.Identity.Manager
{
    /// <summary>
    /// Provides an abstraction for a storage and management of identity users.
    /// </summary>
    /// <typeparam name="TUser">The type that represents an identity user.</typeparam>
    public interface IUserStore<TUser> where TUser : class
    {
        /// <summary>
        /// Create identity user in the user store.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>An asynchronous Task returns true if create operation is successfull, otherwise returns false.</returns>
        Task<bool> CreateAsync(TUser user);

        /// <summary>
        /// Updates identity user in the user store.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <returns>An asynchronous Task returns true if update operation is successfull, otherwise returns false.</returns>
        Task<bool> UpdateAsync(TUser user);

        /// <summary>
        /// Deletes identity user from the user store.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        /// <returns>An asynchronous Task returns true if delete operation is successfull, otherwise returns false.</returns>
        Task<bool> DeleteAsync(TUser user);

        /// <summary>
        /// Finds and returns a identity user that matches the userId">.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <returns>An asynchronous Task returns the user if it exists, otherwise returns null.</returns>
        Task<TUser> FindByIdAsync(Guid userId);

        /// <summary>
        /// Finds and returns a identity user that matches email.
        /// </summary>
        /// <param name="email">The email to search for.</param>
        /// <returns>An asynchronous Task returns the user if it exists, otherwise returns null.</returns>
        Task<TUser> FindByEmailAsync(string email);

        /// <summary>
        /// Validate identity user using email and password.
        /// </summary>
        /// <param name="email">The email of the identity user to validate.</param>
        /// <param name="password">The password of the identity user to validate.</param>
        /// <returns>An asynchronous Task returns true if the identity is validated, otherwise returns false.</returns>
        Task<bool> ValidateIdentityAsync(string email, string password);
    }
}
