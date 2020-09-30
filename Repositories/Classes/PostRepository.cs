using Microsoft.EntityFrameworkCore;
using SocialAPI.Models;
using SocialAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.Repositories.Classes
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _db;
        public PostRepository(DataContext db) => _db = db;

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _db.Posts.Include(post => post.Comments)
                                   .Include(post => post.PostCategories).ThenInclude(postcategories => postcategories.Category)
                                   .ToListAsync();
        }

        public async Task<Post> GetByIdAsync(Guid id)
        {
            return await _db.Posts.Include(post => post.Comments)
                                  .Include(post => post.PostCategories).ThenInclude(postCategories => postCategories.Category)
                                  .FirstOrDefaultAsync(post => post.Id == id);
        }

        public async Task<Post> CreateAsync(Post post)
        {
            await _db.Posts.AddAsync(post);
            return post;
        }

        public void Update(Post post)
        {
            _db.Posts.Update(post);
        }

        public void Delete(Post post)
        {
            _db.Posts.Remove(post);
        }
    }
}
