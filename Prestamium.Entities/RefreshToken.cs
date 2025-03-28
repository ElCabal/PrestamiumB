using System.ComponentModel.DataAnnotations.Schema;

namespace Prestamium.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = default!;
    }
}
