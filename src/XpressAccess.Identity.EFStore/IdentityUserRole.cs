using System;

namespace XpressAccess.Identity.EFStore
{
    /// <summary>
    /// Represents the link between an identity user and an identity role.
    /// </summary>
    public class IdentityUserRole
    {
        /// <summary>
        /// Gets or sets the primary key of the user that is linked to a role.
        /// </summary>
        public virtual Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the primary key of the role that is linked to the user.
        /// </summary>
        public virtual Guid RoleId { get; set; }
    }
}
