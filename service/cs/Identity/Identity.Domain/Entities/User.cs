#nullable disable

using Identity.Domain.Enums;

namespace Identity.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Oid { get; set; }

        public Role Role { get; set; }

        public bool Active { get; set; } = true;
    }
}

