using System;
using System.Threading.Tasks;

namespace XpressAccess.Identity.Manager
{
    /// <summary>
    /// Provides an abstraction for a storage and management of identity roles.
    /// </summary>
    /// <typeparam name="TRole">The type that represents an identity role.</typeparam>
    public interface IRoleStore<TRole> where TRole : class
    {
        /// <summary>
        /// Creates identity role in a role store.
        /// </summary>
        /// <param name="role">The role to create</param>
        /// <returns>An asynchronous Task returns true if create operation is successfull, otherwise returns false.</returns>
        Task<bool> CreateAsync(TRole role);

        /// <summary>
        /// Updates identity role in a role store.
        /// </summary>
        /// <param name="role">The role to update</param>
        /// <returns>An asynchronous Task returns true if update operation is successfull, otherwise returns false.</returns>
        Task<bool> UpdateAsync(TRole role);

        /// <summary>
        /// Deletes identity role in a role store.
        /// </summary>
        /// <param name="role">The role to delete</param>
        /// <returns>An asynchronous Task returns true if delete operation is successfull, otherwise returns false.</returns>
        Task<bool> DeleteAsync(TRole role);

        /// <summary>
        /// Finds and returns a identity role that matches the roleId">.
        /// </summary>
        /// <param name="roleId">The role ID to search for.</param>
        /// <returns>An asynchronous Task returns the role if it exists, otherwise returns null.</returns>
        Task<TRole> FindByIdAsync(Guid roleId);

        /// <summary>
        /// Finds and returns a identity role that matches roleName.
        /// </summary>
        /// <param name="roleName">The IdentityRole name to search for.</param>
        /// <returns>An asynchronous Task returns the role if it exists, otherwise returns null.</returns>
        Task<TRole> FindByNameAsync(string roleName);
    }
}
