using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialAPI.DTOs;
using SocialAPI.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReturnedPostDTO>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<ReturnedPostDTO>> GetAll()
        {
            return await _postService.GetAllPostsAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReturnedPostDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReturnedPostDTO>> GetById(Guid id)
        {
            var result = await _postService.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return result.Data;
        }

        [HttpGet("{id}/comments/{commentId}")]
        [ProducesResponseType(typeof(CommentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CommentDTO>> GetByIdComment(Guid id, Guid commentId)
        {
            var result = await _postService.GetByIdWithCommentAsync(id, commentId);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return result.Data;
        }


        [HttpPost]
        [Authorize(Roles = "Common")]
        [ProducesResponseType(typeof(PostDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] PostDTO postDTO)
        {
            var result = await _postService.CreateAsync(postDTO);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction("GetById",new { postDTO.Id }, result.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Common")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] PostDTO postDTO)
        {
            var result = await _postService.UpdateAsync(id, postDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Common")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _postService.Delete(id);
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
