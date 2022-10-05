#nullable disable

using Identity.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Domain.Entities
{
    public class User : Base
    {
        public string Oid { get; set; }

        public Role Role { get; set; }

        public bool Active { get; set; } = true;
    }

    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> modelBuilder)
        {
            modelBuilder.ToContainer("user");
            modelBuilder
                .Property(u => u.Oid)
                .IsRequired();
            modelBuilder
                .HasIndex(u => new { u.Id, u.Oid })
                .IsUnique();
            modelBuilder
                .Property(u => u.Role)
                .IsRequired();
            modelBuilder
                .Property(u => u.Created)
                .IsRequired();
            modelBuilder
                .Property(u => u.CreatedByOid)
                .IsRequired();
            modelBuilder
                .Property(u => u.Modified)
                .IsRequired();
            modelBuilder
                .Property(u => u.ModifiedByOid)
                .IsRequired();
        }
    }
}