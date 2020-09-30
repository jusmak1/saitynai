using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.Models
{
    public class PostCategories
    {

        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }

        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
