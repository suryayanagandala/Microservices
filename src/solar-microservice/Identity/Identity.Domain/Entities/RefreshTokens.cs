using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entities
{
    public class RefreshTokens
    {
        [Key]
        public int TokenId { get; set; }

        [Required]
        [StringLength(6)]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;


        public DateTimeOffset CreatedAt { get; set; }= DateTimeOffset.Now;

        public DateTimeOffset ExpiryDate { get; set; }

        public DateTimeOffset? RevokedAt {  get; set; }

        public bool IsRevoked {  get; set; }=false;

        public string? ReplacedByToken { get; set; }
        public Users Users { get; set; } = default!;    
    }
}
