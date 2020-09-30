using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.DTOs.User
{
    public class AuthenticatedUserDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
