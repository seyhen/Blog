using Blog.Services;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using System.Reflection.Metadata;
using Newtonsoft.Json;

namespace Blog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _authorService;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(AuthorService AuthorService, ILogger<AuthorController> logger)    
        {
            _authorService = AuthorService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Author>> GetAuthors()
        {
            _logger.LogInformation("Getting all authors");
            var authors = _authorService.Get();
            return Ok(authors);
        }

        [HttpGet("CountPublishedBlog/{id}")]
        public ActionResult<int> CountPublishedBlog(string id)
        {
            try
            {
                _logger.LogInformation($"Getting number of published blog for author id {id}");
                var count = _authorService.CountPublishedBlog(id);

                return Ok(count);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Author> GetAuthor(string id)
        {
            try
            {
                _logger.LogInformation($"Getting number of published blog for author id {id}");
                var author = _authorService.Get(id);
                return author;
            } 
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<Author> PostAuthor(Author author)
        {
            _logger.LogInformation($"Create author {JsonConvert.SerializeObject(author)}");

            if (string.IsNullOrEmpty(author.Name))
            {
                _logger.LogError($"The name of author is missing");
                return BadRequest("The name of author is mandatory");
            }

            _authorService.Create(author);
            return CreatedAtAction("PostAuthor", new { id = author.Id }, author);
        }
    }
}