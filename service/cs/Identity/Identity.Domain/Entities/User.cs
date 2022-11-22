using Identity.Domain.Attributes;
using Identity.Domain.Enums;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Domain.Entities
{
    public class User : Base
    {
        [PatchProtected(Accesslevel = (int) Role.Root, AllowedOperationTypes = new[] { (int) OperationType.Replace })]
        public Role Role { get; set; }

        [PatchProtected(Accesslevel = (int) Role.Root, AllowedOperationTypes = new[] { (int) OperationType.Replace })]
        public bool Active { get; set; } = true;
    }

    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> modelBuilder)
        {
            modelBuilder.ToContainer("user");
            modelBuilder.HasKey(u => u.Oid);
            modelBuilder
                .Property(u => u.Oid)
                .IsRequired();
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