using Microsoft.EntityFrameworkCore;

namespace XpressAccess.Identity.EFStore
{
    /// <summary>
    /// Entity Framework database context used for identity system.
    /// </summary>
    public class IdentityDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of IdentityDbContext.
        /// </summary>
        /// <param name="options">The options to be used by DbContext>.</param>
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        { }

        /// <summary>
        /// Initializes a new instance of the IdentityDbContext class.
        /// </summary>
        protected IdentityDbContext()
        { }

        /// <summary>
        /// Gets or sets the DbSet of Users.
        /// </summary>
        public DbSet<IdentityUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of User roles.
        /// </summary>
        public DbSet<IdentityUserRole> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of roles.
        /// </summary>
        public DbSet<IdentityRole> Roles { get; set; }

        /// <summary>
        /// Configures the schema needed for the identity framework.
        /// </summary>
        /// <param name="builder">
        /// The builder being used to construct the model for this context.
        /// </param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUser>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.Email).HasName("EmailIndex").IsUnique();
                b.ToTable("XAIdentity");
                b.Property(u => u.Email).HasMaxLength(256);
                b.HasMany(u => u.Roles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.HasKey(r => r.Id);
                b.ToTable("XARole");
                b.Property(r => r.Name).HasMaxLength(32);
                b.HasMany(r => r.Users).WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
            });

            builder.Entity<IdentityUserRole>(b =>
            {
                b.HasKey(ur => new { ur.UserId, ur.RoleId });
                b.ToTable("XAUserRole");
            });
        }
    }
}
