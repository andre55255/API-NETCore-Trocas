using Microsoft.AspNetCore.Identity;

namespace ExchangeApp.Core.Entities
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Whatsapp { get; set; }
        public string Instagram { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DisabledAt { get; set; }
    }
}
