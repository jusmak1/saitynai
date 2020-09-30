using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialAPI.DTOs;
using SocialAPI.Services.Interfaces;

namespace SocialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoriesService;

        public CategoriesController(ICategoryService categoryService) => _categoriesService = categoryService;

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDTO>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            return await _categoriesService.GetAllCategoriesAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDTO>> GetById(Guid id)
        {
            var result = await _categoriesService.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return result.Data;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CategoryDTO categoryDTO)
        {
            var result = await _categoriesService.CreateAsync(categoryDTO);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction("GetById", new { categoryDTO.Id }, result.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryDTO categoryDTO)
        {
            var result = await _categoriesService.UpdateAsync(id, categoryDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _categoriesService.Delete(id);
            if (!result.Success)
            {
                if (result.ResponseType == Helpers.ResponseType.NotFound)
                    return NotFound(result.Message);
                return BadRequest(result.Message);
            }
            return Ok();
        }

    }
}
