﻿#nullable disable

namespace Identity.API.Configurations
{
    public record LocalMSSqlSection
    {
        public string Server { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string DatabaseName { get; set; }
    }
}
