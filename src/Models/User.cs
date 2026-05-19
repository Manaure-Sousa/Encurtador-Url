using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace EncurtadorUrl.src.Models
{
    public class User
    {
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public ICollection<ShortenedUrl> ShortenedUrls { get; set; } = [];
    }
}