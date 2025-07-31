using Juju.Application.Contracts.Services;
using Juju.Application.Dtos;
using Juju.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Post
{
    public class PostController : BaseController
    {
        private readonly IPostServices _postService;
        public PostController(IPostServices postService)
        {
            _postService = postService;
        }

        [HttpGet("GetPosts")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _postService.GetAll();
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpPost("CreatePost")]
        public async Task<IActionResult> Create([FromBody] PostRequest entity)
        {
            var result = await _postService.CreatePost(entity);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpPut("UpdatePost")]
        public async Task<IActionResult> Update([FromBody] PostUpdate entity)
        {
            var result = await _postService.UpdatePost(entity);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpDelete("DeletePost/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _postService.DeletePost(Id);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpPost("CreateManyPosts")]
        public async Task<IActionResult> CreateMany([FromBody] List<PostRequest> entities)
        {
            var result = await _postService.CreateManyPosts(entities);
            return StatusCode((int)result.HttpStatusCode, result);
        }


    }
}
