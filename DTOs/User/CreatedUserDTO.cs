using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
    
namespace SocialAPI.DTOs.User
{
    public class CreatedUserDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
