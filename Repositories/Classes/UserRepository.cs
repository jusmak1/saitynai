using Microsoft.EntityFrameworkCore;
using SocialAPI.Models;
using SocialAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.Repositories.Classes
{
    public class UserRepository : IUserRepository
    {

        private readonly DataContext _db;

        public UserRepository(DataContext db)
        {
            _db = db;
        }

        public async Task AddAsync(User user, ERole[] userRoles)
        {
            var roleNames = userRoles.Select(r => r.ToString()).ToList();
            var roles = await _db.Roles.Where(r => roleNames.Contains(r.Name)).ToListAsync();

            foreach (var role in roles)
            {
                user.UserRoles.Add(new UserRoles { RoleName = role.Name, UserEmail = user.Email });
            }

            _db.Users.Add(user);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _db.Users.Include(u => u.UserRoles)
                                        .ThenInclude(ur => ur.Role)
                                  .SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}
