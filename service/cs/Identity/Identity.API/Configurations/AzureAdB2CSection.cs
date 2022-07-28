#nullable disable

namespace Identity.API.Configurations
{
    public record AzureAdB2CSection
    {
        public string Instance { get; set; }

        public string Domain { get; set; }

        public string ClientId { get; set; }

        public string SignedOutCallbackPath { get; set; }

        public string SignUpSignInPolicyId { get; set; }
    }
}

