using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Blog.Services;
using Newtonsoft.Json;

namespace Blog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly BlogService _blogService;
        private readonly AuthorService _authorService;
        private readonly ILogger<BlogController> _logger;

        public BlogController(BlogService blogService, AuthorService authorService, ILogger<BlogController> logger)
        {
            _blogService = blogService;
            _authorService = authorService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BlogPost>> GetBlogs()
        {
            _logger.LogInformation("Getting all blogs");
            var blogs = _blogService.Get();
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        public ActionResult<BlogPost> GetBlog(string id)
        {
            try
            {
                _logger.LogInformation($"Get blog id {id}");
                var blog = _blogService.Get(id);
                return blog;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<BlogPost> PostBlog(BlogPost blog)
        {
            _logger.LogInformation($"Create blog {JsonConvert.SerializeObject(blog)}");

            if (string.IsNullOrEmpty(blog.Title))
            {
                _logger.LogError($"Blog title is missing");
                return BadRequest("The title of the blog is mandatory");
            }
            if (string.IsNullOrEmpty(blog.Content))
            {
                _logger.LogError($"Blog content is missing");
                return BadRequest("The content of the blog is mandatory");
            }
            if (string.IsNullOrEmpty(blog.Author))
            {
                _logger.LogError($"Blog author is missing");
                return BadRequest("The author of the blog is mandatory");
            }

            try
            {
                _authorService.Get(blog.Author);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return NotFound();
            }

            _blogService.Create(blog);
            return CreatedAtAction("PostBlog", new { id = blog.Id }, blog);
        }
    }
}