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
    }
}

