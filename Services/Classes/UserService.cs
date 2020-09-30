using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SocialAPI.DTOs.User;
using SocialAPI.Helpers;
using SocialAPI.Models;
using SocialAPI.Repositories.Interfaces;
using SocialAPI.Security;
using SocialAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialAPI.Services.Classes
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        private readonly DataContext _db;

        public UserService(IUserRepository userRepository,
                           IPasswordHasher passwordHasher,
                           IMapper mapper,
                           DataContext db,
                           IConfiguration config)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _mapper = mapper;
            _db = db;
            _config = config;
        }

        public async Task<ServiceResponse<AuthenticatedUserDTO>> AuthenticateUserAsync(PostUserDTO UserCredentials)
        {
            var user = await _userRepository.FindByEmailAsync(UserCredentials.Email);
            if(user == null)
            {
                var errorMessage = $"Password or login is incorrect";
                Log.Error(errorMessage);
                return new ServiceResponse<AuthenticatedUserDTO> { Message = errorMessage, Success = false };
            }

            if(!_passwordHasher.PasswordMatches(UserCredentials.Password, user.Password))
            {
                var errorMessage = $"Password or login is incorrect";
                Log.Error(errorMessage);
                return new ServiceResponse<AuthenticatedUserDTO> { Message = errorMessage, Success = false };
            }

            var token = GenerateJwtToken(user);
            var authenticatedUserDTO = _mapper.Map<AuthenticatedUserDTO>(user);
            authenticatedUserDTO.Token = token;
            return new ServiceResponse<AuthenticatedUserDTO> { Data = authenticatedUserDTO };
        }

        public async Task<ServiceResponse<CreatedUserDTO>> CreateUserAsync(PostUserDTO userCredentials, params ERole[] userRoles)
        {
            var existingUser = await _userRepository.FindByEmailAsync(userCredentials.Email);
            if (existingUser != null)
            {
                string errorMesage = $"User with Email: {existingUser.Email} already exists";
                Log.Error(errorMesage);
                return new ServiceResponse<CreatedUserDTO> { Message = errorMesage, Success = false };
            }

            var user = _mapper.Map<User>(userCredentials);
            user.Password = _passwordHasher.HashPassword(user.Password);

            try
            {
                await _userRepository.AddAsync(user, userRoles);
                await _db.SaveChangesAsync();
            }catch(Exception e)
            {
                Log.Error($"{e.Message}-{e.InnerException?.Message}");
                return new ServiceResponse<CreatedUserDTO> { Message = e.Message, Success = false };
            }

            var createdUser = _mapper.Map<CreatedUserDTO>(await _userRepository.FindByEmailAsync(user.Email));
            createdUser.Token = GenerateJwtToken(user);

            return new ServiceResponse<CreatedUserDTO> { Data = createdUser };
        }

        private string GenerateJwtToken(User user)
        {
            string key = _config.GetSection("AppSettings:Token").Value;
            var issuer = _config.GetSection("AppSettings:Issuer").Value;   
            var expires = DateTime.Now.AddSeconds(int.Parse(_config.GetSection("AppSettings:Expires").Value));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(ClaimTypes.Name, user.Email.ToString()));
            permClaims.AddRange(user.UserRoles.Select(ur => new Claim(ClaimsIdentity.DefaultRoleClaimType, ur.Role.Name)));

            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: expires,
                            signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);           
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userRepository.FindByEmailAsync(email);
        }

        
    }
}
