using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialAPI.DTOs;
using SocialAPI.Helpers;
using SocialAPI.Models;
using SocialAPI.Repositories.Interfaces;
using SocialAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.Services.Classes
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepo;
        private readonly IMapper _mapper;
        private readonly DataContext _db;
        public PostService(IPostRepository postRepo, IMapper mapper, DataContext db)
        {
            _postRepo = postRepo;
            _mapper = mapper;
            _db = db;
        }

        public async Task<IEnumerable<ReturnedPostDTO>> GetAllPostsAsync()
        {
            return _mapper.Map<IEnumerable<Post>, IEnumerable<ReturnedPostDTO>>(await _postRepo.GetAllAsync());
        }
        public async Task<ServiceResponse<ReturnedPostDTO>> GetByIdAsync(Guid id)
        {
            var post = await _postRepo.GetByIdAsync(id);
            if (post == null)
            {
                return new ServiceResponse<ReturnedPostDTO>
                {
                    Success = false,
                    Message = $"Post with Id: ${id} was not found"
                };
            }

            var returnedPost = _mapper.Map<ReturnedPostDTO>(post);
            return new ServiceResponse<ReturnedPostDTO> { Data = returnedPost };
        }

        public async Task<ServiceResponse<PostDTO>> CreateAsync(PostDTO postDTO)
        {
            var post = _mapper.Map<Post>(postDTO);
      
            try
            {
                await _postRepo.CreateAsync(post);
                await _db.SaveChangesAsync();

                if (postDTO.Categories != null)
                {
                    post.PostCategories = postDTO.Categories.Select(categoryDTO => new PostCategories
                    {
                        PostId = post.Id,
                        CategoryId = categoryDTO.Id
                    }).ToList();

                    await _db.SaveChangesAsync();
                }

                postDTO.Id = post.Id;
                return new ServiceResponse<PostDTO> { Data = postDTO };
            }catch(Exception e)
            {
                return new ServiceResponse<PostDTO> 
                { 
                    Success = false, 
                    Message = $"Error creating post. Error message: ${e.Message} ${e.InnerException?.Message ?? ""}" 
                };
            }
        }

        public async Task<ServiceResponse<PostDTO>> UpdateAsync(Guid id, PostDTO postDTO)
        {
            var post = await _postRepo.GetByIdAsync(id);
            if(post == null)
            {
                return new ServiceResponse<PostDTO>
                { 
                    Success = false,
                    Message = $"Post with id: ${id} was not found" 
                };
            }

            post.Text = postDTO.Text;
            post.PostedBy = postDTO.PostedBy;
            post.CreatedAt = postDTO.CreatedAt;
            if(postDTO.Categories != null)
            {
                post.PostCategories = postDTO.Categories.Select(category => new PostCategories
                {
                    PostId = post.Id,
                    CategoryId = category.Id
                }).ToList();
            }

            try
            {
                _postRepo.Update(post);
                await _db.SaveChangesAsync();
                return new ServiceResponse<PostDTO> { Data = postDTO };
            }catch(Exception e)
            {
                return new ServiceResponse<PostDTO>
                { 
                    Success = false,
                    Message= $"Error updating post. Error message ${e.Message} ${e.InnerException?.Message ?? ""}"
                };
            }
        }

        public async Task<ServiceResponse<PostDTO>> Delete(Guid id)
        {
            var post = await _postRepo.GetByIdAsync(id);
            if(post == null)
            {
                return new ServiceResponse<PostDTO>
                {
                    Success = false,
                    Message = $"Post with id ${id} was not found",
                    ResponseType = ResponseType.NotFound
                };
            }
            
            try
            {
                _postRepo.Delete(post);
                await _db.SaveChangesAsync();
                return new ServiceResponse<PostDTO>();
            }catch(Exception e)
            {
                return new ServiceResponse<PostDTO>
                {
                    Success = false,
                    Message = $"Error deleting post. Error message ${e.Message} ${e.InnerException?.Message ?? ""}",
                    ResponseType = ResponseType.BadRequest
                };
            }
        }

        public async Task<ServiceResponse<CommentDTO>> GetByIdWithCommentAsync(Guid id, Guid commentId)
        {
            var post = await _postRepo.GetByIdAsync(id);
            if (post == null)
            {
                return new ServiceResponse<CommentDTO>
                {
                    Success = false,
                    Message = $"Post with Id: ${id} was not found"
                };
            }

            var comment = post.Comments.FirstOrDefault(comment => comment.Id == commentId);
            if(comment == null)
            {
                return new ServiceResponse<CommentDTO>
                {
                    Success = false,
                    Message = $"Comment with Id: ${commentId} was not found for this post"
                };
            }

            var commentResult = _mapper.Map<CommentDTO>(comment);
            return new ServiceResponse<CommentDTO> { Data = commentResult };
        }
    }
}
