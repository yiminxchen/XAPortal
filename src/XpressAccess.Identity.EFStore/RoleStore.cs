using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using XpressAccess.Identity.Manager;

namespace XpressAccess.Identity.EFStore
{
    /// <summary>
    /// Concrete implementation of IRoleStore, provide persistence store for identity role.
    /// </summary>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// /// <typeparam name="TUserRole">The type representing a user/role map.</typeparam>
    /// <typeparam name="TContext">The type of the DbContext used to access the store.</typeparam>
    public class RoleStore<TRole, TContext> :
        IRoleStore<TRole>
        where TRole : IdentityRole
        where TContext : DbContext

    {
        public TContext _context { get; private set; }
        private DbSet<TRole> RoleSet { get { return _context.Set<TRole>(); } }

        /// <summary>
        /// Creates a new instance of identity role Store>.
        /// </summary>
        /// <param name="context">The DbContext used to access the store.</param>
        public RoleStore(TContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates identity role in a role store.
        /// </summary>
        /// <param name="role">The role to create</param>
        /// <returns>An asynchronous Task returns true if create operation is successfull, otherwise returns false.</returns>
        public async virtual Task<bool> CreateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            _context.Add(role);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Updates identity role in a role store.
        /// </summary>
        /// <param name="role">The role to update</param>
        /// <returns>An asynchronous Task returns true if update operation is successfull, otherwise returns false.</returns>
        public async virtual Task<bool> UpdateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            _context.Attach(role);
            _context.Update(role);
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
        /// Deletes identity role in a role store.
        /// </summary>
        /// <param name="role">The role to delete</param>
        /// <returns>An asynchronous Task returns true if delete operation is successfull, otherwise returns false.</returns>
        public async virtual Task<bool> DeleteAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            _context.Remove(role);
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
        /// Finds and returns a identity role that matches the roleId">.
        /// </summary>
        /// <param name="roleId">The role ID to search for.</param>
        /// <returns>An asynchronous Task returns the role if it exists, otherwise returns null.</returns>
        public virtual Task<TRole> FindByIdAsync(Guid roleId)
        {
            return RoleSet.FirstOrDefaultAsync(r => r.Id == roleId);
        }

        /// <summary>
        /// Finds and returns a identity role that matches roleName.
        /// </summary>
        /// <param name="roleName">The IdentityRole name to search for.</param>
        /// <returns>An asynchronous Task returns the role if it exists, otherwise returns null.</returns>
        public virtual Task<TRole> FindByNameAsync(string roleName)
        {
            return RoleSet.FirstOrDefaultAsync(r => r.Name == roleName);
        }
    }
}
