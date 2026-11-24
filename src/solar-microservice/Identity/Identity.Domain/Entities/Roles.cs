using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entities
{
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }

        [Required, StringLength(50)]
        
        public required string RoleName { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        public ICollection<Users> Users { get; set; } = new List<Users>();
    }
}
