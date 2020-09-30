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
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CommentDTO>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<CommentDTO>> GetAll()
        {
            return await _commentService.GetAllCommentsAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CommentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CommentDTO>> GetById(Guid id)
        {
            var result = await _commentService.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return result.Data;
        }

        [HttpPost]
        [Authorize(Roles = "Common")]
        [ProducesResponseType(typeof(CommentDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CommentDTO commentDTO)
        {
            var result = await _commentService.CreateAsync(commentDTO);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction("GetById", new { commentDTO.Id }, result.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Common")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CommentDTO commentDTO)
        {
            var result = await _commentService.UpdateAsync(id, commentDTO);
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
            var result = await _commentService.Delete(id);
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
