using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Data;
using TeacherWebsiteBackEnd.Entities;

namespace TeacherWebsiteBackEnd.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            IEnumerable<Post> posts = await _postService.GetPosts();
            if (posts == null) return NotFound();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPostById(int id)
        {
            Post post = await _postService.GetPostById(id);
            if (post == null) return NotFound();
            return Ok(post);
        }

        //[Authorize]
        [HttpPost]
        public async Task<ActionResult<Post>> AddPost([FromBody] Post post)
        {
            Post _post = await _postService.AddPost(post);
            if (_post == null) return BadRequest();
            return Created($"api/posts/{_post.Id}", _post);
        }

        //[Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Post>> ReplacePost([FromBody] Post post)
        {
            Post _post = await _postService.ReplacePost(post);
            if (_post == null) return NotFound();
            return Ok(_post);
        }

        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePostById(int id)
        {
            bool success = await _postService.DeletePostById(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
