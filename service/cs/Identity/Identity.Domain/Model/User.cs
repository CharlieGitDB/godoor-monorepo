using System;
namespace Identity.Domain
{
	public class User
	{
		public int Id { get; set; }

		public string Oid { get; set; }

		public Role Role { get; set; }

		public bool Active { get; set; } = true;
	}
}

