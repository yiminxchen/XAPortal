using System.Collections.Generic;
using System.Threading.Tasks;

namespace XpressAccess.Identity.Manager
{
    /// <summary>
    /// Provides an abstraction for a store which maps identity users to identity roles.
    /// </summary>
    /// <typeparam name="TUser">The type that represents an identity user.</typeparam>
    public interface IUserRoleStore<TUser> : IUserStore<TUser> where TUser : class
    {
        /// <summary>
        /// Assign role to user.
        /// </summary>
        /// <param name="user">The identity user to add the identity role to.</param>
        /// <param name="roleName">The identity role name to add.</param>
        /// <returns>An asynchronous Task</returns>
        Task AssignToRoleAsync(TUser user, string roleName);

        /// <summary>
        /// Removes role from user.
        /// </summary>
        /// <param name="user">The identity user to remove the identity role from.</param>
        /// <param name="roleName">The identity role to remove.</param>
        /// <returns>An asynchronous Task</returns>
        Task RemoveFromRoleAsync(TUser user, string roleName);

        /// <summary>
        /// Get all roles that the user belongs to
        /// </summary>
        /// <param name="user">The identity user to get roles.</param>
        /// <returns>An asynchronous Task returns a list of identity role for the identity user.</returns>
        Task<IList<string>> GetRolesAsync(TUser user);

        /// <summary>
        /// check if the user is a member of the role.
        /// </summary>
        /// <param name="user">The identity user to check.</param>
        /// <param name="roleName">The identity role to check.</param>
        /// <returns>An asynchronous Task returns true if the identity user belongs to the identity role, otherwise returns false </returns>
        Task<bool> IsInRoleAsync(TUser user, string roleName);
    }
}
