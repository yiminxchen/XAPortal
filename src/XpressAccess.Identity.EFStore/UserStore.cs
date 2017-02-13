using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XpressAccess.Identity.Manager;
using Microsoft.Extensions.Internal;

namespace XpressAccess.Identity.EFStore
{
    /// <summary>
    /// Concrete implementation of IUserStore and IUserRoleStore, provide persistence store for identity user.
    /// </summary>
    /// <typeparam name="TUser">The type representing a identity user.</typeparam>
    /// <typeparam name="TRole">The type representing a identity role.</typeparam>
    /// <typeparam name="TUserRole">The type representing link between identity user and role.</typeparam>
    /// <typeparam name="TContext">The type of the DbContext used to access the store.</typeparam>
    public class UserStore<TUser, TRole, TUserRole, TContext> :
        IUserStore<TUser>,
        IUserRoleStore<TUser>,
        IPasswordStore<TUser>
        where TUser : IdentityUser
        where TRole : IdentityRole
        where TUserRole : IdentityUserRole, new()
        where TContext : DbContext
    {
        public TContext _context { get; private set; }

        private DbSet<TUser> UserSet { get { return _context.Set<TUser>(); } }
        private DbSet<TRole> RoleSet { get { return _context.Set<TRole>(); } }
        private DbSet<TUserRole> UserRoleSet { get { return _context.Set<TUserRole>(); } }

        /// <summary>
        /// Creates a new instance of identity user Store>.
        /// </summary>
        /// <param name="context">The DbContext used to access the store.</param>
        public UserStore(TContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create identity user in the user store.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>An asynchronous Task returns true if create operation is successfull, otherwise returns false.</returns>
        public async virtual Task<bool> CreateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            
            _context.Add(user);

            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Updates identity user in the user store.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <returns>An asynchronous Task returns true if update operation is successfull, otherwise returns false.</returns>
        public async virtual Task<bool> UpdateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Attach(user);
            _context.Update(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Deletes identity user from the user store.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        /// <returns>An asynchronous Task returns true if delete operation is successfull, otherwise returns false.</returns>
        public async virtual Task<bool> DeleteAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Remove(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Finds and returns a identity user that matches the userId">.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <returns>An asynchronous Task returns the user if it exists, otherwise returns null.</returns>
        public virtual Task<TUser> FindByIdAsync(Guid userId)
        {
            return UserSet.FindAsync(new object[] { userId });
        }

        /// <summary>
        /// Finds and returns a identity user that matches email.
        /// </summary>
        /// <param name="email">The email to search for.</param>
        /// <returns>An asynchronous Task returns the user if it exists, otherwise returns null.</returns>
        public virtual Task<TUser> FindByEmailAsync(string email)
        {
            return UserSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Validate identity user using email and password.
        /// </summary>
        /// <param name="email">The email of the identity user to validate.</param>
        /// <param name="password">The password of the identity user to validate.</param>
        /// <returns>An asynchronous Task returns true if the identity is validated, otherwise returns false.</returns>
        public virtual Task<bool>  ValidateIdentityAsync(string email, string password)
        {
            var user = FindByEmailAsync(email).Result;
            if (user != null)
            {
                if (user.AccessAllowed && user.PasswordHash == password)
                {
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false); ;
        }

        /// <summary>
        /// Assign role to user.
        /// </summary>
        /// <param name="user">The identity user to add the identity role to.</param>
        /// <param name="roleName">The identity role name to add.</param>
        /// <returns>An asynchronous Task</returns>
        public async virtual Task AssignToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("cannot be null orempty", nameof(roleName));
            }
            var role = await FindRoleAsync(roleName);
            if (role == null)
            {
                throw new InvalidOperationException($"{roleName} not found");
            }
            UserRoleSet.Add(CreateUserRole(user, role));
        }

        /// <summary>
        /// Removes role from user.
        /// </summary>
        /// <param name="user">The identity user to remove the identity role from.</param>
        /// <param name="roleName">The identity role to remove.</param>
        /// <returns>An asynchronous Task</returns>
        public async virtual Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("cannot be null orempty", nameof(roleName));
            }
            var role = await FindRoleAsync(roleName);
            if (role != null)
            {
                var userRole = await FindUserRoleAsync(user.Id, role.Id);
                if (userRole != null)
                {
                    UserRoleSet.Remove(userRole);
                }
            }
        }

        /// <summary>
        /// Get all roles that the user belongs to
        /// </summary>
        /// <param name="user">The identity user to get roles.</param>
        /// <returns>An asynchronous Task returns a list of identity role for the identity user.</returns>
        public virtual async Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var query = from userRole in UserRoleSet
                        join role in RoleSet on userRole.RoleId equals role.Id
                        where userRole.UserId.Equals(user.Id)
                        select role.Name;

            return await query.ToListAsync();
        }

        /// <summary>
        /// check if the user is a member of the role.
        /// </summary>
        /// <param name="user">The identity user to check.</param>
        /// <param name="roleName">The identity role to check.</param>
        /// <returns>An asynchronous Task returns true if the identity user belongs to the identity role, otherwise returns false </returns>
        public virtual async Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("cannot be null or empty", nameof(roleName));
            }
            var role = await FindRoleAsync(roleName);
            if (role != null)
            {
                var userRole = await FindUserRoleAsync(user.Id, role.Id);
                return userRole != null;
            }
            return false;
        }

        /// <summary>
        /// Create a new instance of identity UserRole.
        /// </summary>
        /// <param name="user">The identity user.</param>
        /// <param name="role">The identity role.</param>
        /// <returns></returns>
        protected virtual TUserRole CreateUserRole(TUser user, TRole role)
        {
            return new TUserRole()
            {
                UserId = user.Id,
                RoleId = role.Id
            };
        }

        /// <summary>
        /// Find identity role using role name
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <returns>An asynchronous Task returns the identity role if it exists, otherwise returns null.</returns>
        protected virtual Task<TRole> FindRoleAsync(string roleName)
        {
            return RoleSet.SingleOrDefaultAsync(r => r.Name == roleName);
        }

        /// <summary>
        /// Find identity user and role map using user ID and role ID
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <returns>An asynchronous Task returns the identity UserRole entity if it exists, otherwise returns null.</returns>
        protected virtual Task<TUserRole> FindUserRoleAsync(Guid userId, Guid roleId)
        {
            return UserRoleSet.FindAsync(new object[] { userId, roleId });
        }

        /// <summary>
        /// Set passsword for identity user in the user store.
        /// </summary>
        /// <param name="user">The identity user.</param>
        /// <returns>An asynchronous Task.</returns>
        public virtual Task SetPasswordAsync(TUser user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.PasswordHash = password;

            return Task.CompletedTask;
        }

        /// <summary>
        /// Get passsword for identity user in the user store.
        /// </summary>
        /// <param name="user">The identity user.</param>
        /// <returns>An asynchronous Task, returns identity user's password.</returns>
        public virtual Task<string> GetPasswordAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.PasswordHash);
        }
    }
}
