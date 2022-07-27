using System;
using Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data
{
	public class UserContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
			dbContextOptionsBuilder.UseSqlServer(
				"Data Source= (localdb)\\MSSQLLocalDB; Inital Catalog=IdentityData");
			base.OnConfiguring(dbContextOptionsBuilder);
        }
	}
}

