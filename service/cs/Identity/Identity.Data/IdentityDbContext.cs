using Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data
{
  public class IdentityDbContext : DbContext
  {
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
    {
      dbContextOptionsBuilder.UseSqlServer(
        "Server=localhost;User Id=sa;Password=very_cool_password_ok;Database=IdentityService");
      base.OnConfiguring(dbContextOptionsBuilder);
    }
  }
}

