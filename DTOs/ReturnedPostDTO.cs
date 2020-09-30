using SocialAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.DTOs
{
    public class ReturnedPostDTO
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string PostedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<CommentDTO> Comments { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }
    }
}
