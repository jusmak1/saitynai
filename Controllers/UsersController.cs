using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialAPI.DTOs.User;
using SocialAPI.Models;
using SocialAPI.Services.Interfaces;

namespace SocialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UsersController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreatedUserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] PostUserDTO userCredentials)
        {
            var response = await _userService.CreateUserAsync(userCredentials, ERole.Common);
            if (response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(AuthenticatedUserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authenticate([FromBody] PostUserDTO userCredentials) 
        {
            var repsonse = await _userService.AuthenticateUserAsync(userCredentials);
            if(repsonse.Success == false)
            {
                return BadRequest(repsonse.Message);
            }
            return Ok(repsonse.Data);    
        }
    }
}