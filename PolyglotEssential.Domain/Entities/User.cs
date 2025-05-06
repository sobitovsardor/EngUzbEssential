using PolyglotEssential.Domain.Common;

namespace PolyglotEssential.Domain.Entities
{
    public class User : Auditable
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public string AccountImagePath { get; set; } = string.Empty;
        public int Point { get; set; }
    }
}
