using System;
using System.Collections.Generic;

namespace XpressAccess.Identity.EFStore
{
    /// <summary>
    /// Represents a identity role that a identity use can belong to.
    /// </summary>
    public class IdentityRole
    {
        /// <summary>
        /// Gets or sets the primary key for this role.
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name for this role.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Navigation property for the users in this role.
        /// </summary>
        public virtual ICollection<IdentityUserRole> Users { get; } = new List<IdentityUserRole>();

    }
}
