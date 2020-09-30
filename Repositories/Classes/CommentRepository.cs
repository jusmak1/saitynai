using Microsoft.EntityFrameworkCore;
using SocialAPI.Models;
using SocialAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.Repositories.Classes
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _db;

        public CommentRepository(DataContext db)
        {
            _db = db;
        }
        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _db.Comments.AddAsync(comment);
            return comment;
        }

        public void Delete(Comment comment)
        {
            _db.Comments.Remove(comment);
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _db.Comments.ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(Guid id)
        {
            return await _db.Comments.FirstOrDefaultAsync(comment => comment.Id == id);

        }

        public void Update(Comment comment)
        {
            _db.Comments.Update(comment);

        }
    }
}
