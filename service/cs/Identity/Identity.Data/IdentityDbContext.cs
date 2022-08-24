using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Identity.Data
{
    public class IdentityDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Oid)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .IsRequired();
        }
    }
}

