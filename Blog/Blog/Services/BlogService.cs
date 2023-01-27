using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace Blog.Services
{
    public class BlogService : IService<BlogPost>
    {
        private readonly IMongoDatabase _database;

        public BlogService(string connectionString)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("blogDB");
        }

        public  IEnumerable<BlogPost> Get() {
            return _database.GetCollection<BlogPost>("BlogPosts").AsQueryable();
        }

        public BlogPost Get(string id)
        {
            var blogs = _database.GetCollection<BlogPost>("BlogPosts");
            var blog = blogs.Find(x => x.Id == id).FirstOrDefault();

            if (blog == null)
                throw new Exception("Blog not found");

            return blog;
        }

        public void Create(BlogPost blog)
        {
            blog.Id = Guid.NewGuid().ToString();
            _database.GetCollection<BlogPost>("BlogPosts").InsertOne(blog);
        }
    }
}
