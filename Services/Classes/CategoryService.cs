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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;
        private readonly DataContext _db;

        public CategoryService(DataContext db, IMapper mapper, ICategoryRepository categoryRepo)
        {
            _db = db;
            _mapper = mapper;
            _categoryRepo = categoryRepo;
        }
        public async Task<ServiceResponse<CategoryDTO>> CreateAsync(CategoryDTO categoryDTO)
        {
            var category = _mapper.Map<Category>(categoryDTO);
            try
            {
                await _categoryRepo.CreateAsync(category);
                await _db.SaveChangesAsync();
                categoryDTO.Id = category.Id;
                return new ServiceResponse<CategoryDTO> { Data = categoryDTO };
            }
            catch (Exception e)
            {
                return new ServiceResponse<CategoryDTO>
                {
                    Success = false,
                    Message = $"Error creating coment. Error message: ${e.Message} ${e.InnerException?.Message ?? ""}"
                };
            }
        }

        public async Task<ServiceResponse<CategoryDTO>> Delete(Guid id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
            {
                return new ServiceResponse<CategoryDTO>
                {
                    Success = false,
                    Message = $"Comment with id ${id} was not found",
                    ResponseType = ResponseType.NotFound
                };
            }

            try
            {
                _categoryRepo.Delete(category);
                await _db.SaveChangesAsync();
                return new ServiceResponse<CategoryDTO>();
            }
            catch (Exception e)
            {
                return new ServiceResponse<CategoryDTO>
                {
                    Success = false,
                    Message = $"Error deleting comment. Error message ${e.Message} ${e.InnerException?.Message ?? ""}",
                    ResponseType = ResponseType.BadRequest
                };
            }
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(await _categoryRepo.GetAllAsync());

        }

        public async Task<ServiceResponse<CategoryDTO>> GetByIdAsync(Guid id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
            {
                return new ServiceResponse<CategoryDTO>
                {
                    Success = false,
                    Message = $"Category with Id: ${id} was not found"
                };
            }

            var returnedPost = _mapper.Map<CategoryDTO>(category);
            return new ServiceResponse<CategoryDTO> { Data = returnedPost };
        }

        public async Task<ServiceResponse<CategoryDTO>> UpdateAsync(Guid id, CategoryDTO categoryDTO)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
            {
                return new ServiceResponse<CategoryDTO>
                {
                    Success = false,
                    Message = $"Comment with id: ${id} was not found"
                };
            }

            category.Name = categoryDTO.Name;

            try
            {
                _categoryRepo.Update(category);
                await _db.SaveChangesAsync();
                return new ServiceResponse<CategoryDTO> { Data = categoryDTO };
            }
            catch (Exception e)
            {
                return new ServiceResponse<CategoryDTO>
                {
                    Success = false,
                    Message = $"Error updating comment. Error message ${e.Message} ${e.InnerException?.Message ?? ""}"
                };
            }
        }
    }
}
