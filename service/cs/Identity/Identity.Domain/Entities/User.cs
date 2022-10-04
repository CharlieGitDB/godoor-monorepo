#nullable disable

using Identity.Domain.Enums;

namespace Identity.Domain.Entities
{
    public class User : Base
    {
        public string Oid { get; set; }

        public Role Role { get; set; }

        public bool Active { get; set; } = true;
    }
}

