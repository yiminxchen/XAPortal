using System;
using System.Collections.Generic;

namespace XpressAccess.Identity.EFStore
{
    /// <summary>
    /// Represents a user in the identity system
    /// </summary>
    public class IdentityUser
    {
        /// <summary>
        /// Gets or sets the primary key for this user.
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Gets or sets a salted and hashed representation of the password for this user.
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their email address.
        /// </summary>
        /// <value>True if the email address has been confirmed, otherwise false.</value>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if a user is allowed to access the system
        /// </summary>
        public virtual bool AccessAllowed { get; set; } = true;

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<IdentityUserRole> Roles { get; } = new List<IdentityUserRole>();
    }
}
