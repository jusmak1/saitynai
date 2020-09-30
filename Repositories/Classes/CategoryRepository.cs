using Microsoft.EntityFrameworkCore;
using SocialAPI.Models;
using SocialAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.Repositories.Classes
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _db;

        public CategoryRepository(DataContext db)
        {
            _db = db;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await _db.Categories.AddAsync(category);
            return category;
        }

        public void Delete(Category category)
        {
            _db.Categories.Remove(category);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            return await _db.Categories.FirstOrDefaultAsync(category => category.Id == id);
        }

        public void Update(Category category)
        {
            _db.Update(category);
        }
    }
}
