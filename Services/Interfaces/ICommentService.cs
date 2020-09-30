using SocialAPI.DTOs;
using SocialAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDTO>> GetAllCommentsAsync();
        Task<ServiceResponse<CommentDTO>> GetByIdAsync(Guid id);
        Task<ServiceResponse<CommentDTO>> CreateAsync(CommentDTO postDTO);
        Task<ServiceResponse<CommentDTO>> UpdateAsync(Guid id, CommentDTO postDTO);
        Task<ServiceResponse<CommentDTO>> Delete(Guid id);
    }
}
