using SocialAPI.DTOs;
using SocialAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.Services.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<ReturnedPostDTO>> GetAllPostsAsync();
        Task<ServiceResponse<ReturnedPostDTO>> GetByIdAsync(Guid id);
        Task<ServiceResponse<CommentDTO>> GetByIdWithCommentAsync(Guid id, Guid commentId);
        Task<ServiceResponse<PostDTO>> CreateAsync(PostDTO postDTO);
        Task<ServiceResponse<PostDTO>> UpdateAsync(Guid id, PostDTO postDTO);
        Task<ServiceResponse<PostDTO>> Delete(Guid id);
        
    }
}
