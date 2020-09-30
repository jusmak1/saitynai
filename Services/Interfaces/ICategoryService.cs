using SocialAPI.DTOs;
using SocialAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<ServiceResponse<CategoryDTO>> GetByIdAsync(Guid id);
        Task<ServiceResponse<CategoryDTO>> CreateAsync(CategoryDTO categoryDTO);
        Task<ServiceResponse<CategoryDTO>> UpdateAsync(Guid id, CategoryDTO categoryDTO);
        Task<ServiceResponse<CategoryDTO>> Delete(Guid id);
    }
}
