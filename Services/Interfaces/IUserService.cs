using SocialAPI.DTOs.User;
using SocialAPI.Helpers;
using SocialAPI.Models;
using System.Threading.Tasks;

namespace SocialAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<AuthenticatedUserDTO>> AuthenticateUserAsync(PostUserDTO UserCredentials);
        Task<ServiceResponse<CreatedUserDTO>> CreateUserAsync(PostUserDTO UserCredentials, params ERole[] userRoles);
        Task<User> FindByEmailAsync(string email);
    }
}
