using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.DTOs
{
    public class PostDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Text is required")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Posted by is required")]
        public string PostedBy { get; set; }

        [Required(ErrorMessage = "Created at is required")]
        public DateTime CreatedAt { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }

    }
}
