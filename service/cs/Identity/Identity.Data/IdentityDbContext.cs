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
                .ToContainer("user");
            modelBuilder.Entity<User>()
                .HasPartitionKey(u => u.Id);
            modelBuilder.Entity<User>()
                .Property(u => u.Oid)
                .IsRequired();
            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.Id, u.Oid })
                .IsUnique();
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.Created)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedByOid)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.Modified)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.ModifiedByOid)
                .IsRequired();
            
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is Base && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((Base) entity.Entity).Created = DateTime.UtcNow;
                }

                ((Base) entity.Entity).Modified = DateTime.UtcNow;
                ((Base) entity.Entity).Id = Guid.NewGuid();
            }
        }
    }
}

