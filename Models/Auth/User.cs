using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.Models
{
    public class User
    {
        [Key]
        [DataType(DataType.EmailAddress)]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<UserRoles> UserRoles { get; set; } = new Collection<UserRoles>();

        public IEnumerable<Post> Posts { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}
