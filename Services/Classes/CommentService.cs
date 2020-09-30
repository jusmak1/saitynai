using AutoMapper;
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
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepo;
        private readonly DataContext _db;

      
        public CommentService(IMapper mapper, ICommentRepository commentRepo, DataContext db)
        {
            _mapper = mapper;
            _commentRepo = commentRepo;
            _db = db;
        }
        public async Task<IEnumerable<CommentDTO>> GetAllCommentsAsync()
        {
            return _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(await _commentRepo.GetAllAsync());
        }

        public async Task<ServiceResponse<CommentDTO>> GetByIdAsync(Guid id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return new ServiceResponse<CommentDTO>
                {
                    Success = false,
                    Message = $"Comment with Id: ${id} was not found"
                };
            }

            var returnedPost = _mapper.Map<CommentDTO>(comment);
            return new ServiceResponse<CommentDTO> { Data = returnedPost };
        }

        public async Task<ServiceResponse<CommentDTO>> CreateAsync(CommentDTO commentDTO)
        {
            var comment = _mapper.Map<Comment>(commentDTO);
            try
            {
                await _commentRepo.CreateAsync(comment);
                await _db.SaveChangesAsync();
                commentDTO.Id = comment.Id;
                return new ServiceResponse<CommentDTO> { Data = commentDTO };
            }
            catch (Exception e)
            {
                return new ServiceResponse<CommentDTO>
                {
                    Success = false,
                    Message = $"Error creating coment. Error message: ${e.Message} ${e.InnerException?.Message ?? ""}"
                };
            }
        }

        public async Task<ServiceResponse<CommentDTO>> UpdateAsync(Guid id, CommentDTO commentDTO)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return new ServiceResponse<CommentDTO>
                {
                    Success = false,
                    Message = $"Comment with id: ${id} was not found"
                };
            }

            comment.Text = commentDTO.Text;
            comment.PostedBy = commentDTO.PostedBy;
            comment.CreatedAt = commentDTO.CreatedAt;
            comment.PostId = commentDTO.PostId;

            try
            {
                _commentRepo.Update(comment);
                await _db.SaveChangesAsync();
                return new ServiceResponse<CommentDTO> { Data = commentDTO };
            }
            catch (Exception e)
            {
                return new ServiceResponse<CommentDTO>
                {
                    Success = false,
                    Message = $"Error updating comment. Error message ${e.Message} ${e.InnerException?.Message ?? ""}"
                };
            }
        }

        public async Task<ServiceResponse<CommentDTO>> Delete(Guid id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return new ServiceResponse<CommentDTO>
                {
                    Success = false,
                    Message = $"Comment with id ${id} was not found",
                    ResponseType = ResponseType.NotFound
                };
            }

            try
            {
                _commentRepo.Delete(comment);
                await _db.SaveChangesAsync();
                return new ServiceResponse<CommentDTO>();
            }
            catch (Exception e)
            {
                return new ServiceResponse<CommentDTO>
                {
                    Success = false,
                    Message = $"Error deleting comment. Error message ${e.Message} ${e.InnerException?.Message ?? ""}",
                    ResponseType = ResponseType.BadRequest
                };
            }
        }
    }
}
