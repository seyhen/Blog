using Blog.Models;
using MongoDB.Driver;

namespace Blog.Services
{
    public class AuthorService : IService<Author>
    {
        private readonly IMongoDatabase _database;
        public AuthorService(string connectionString)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("blogDB");
        }

        public IEnumerable<Author> Get()
        {
            return _database.GetCollection<Author>("Authors").AsQueryable();
        }

        public Author Get(string id)
        {
            var authors = _database.GetCollection<Author>("Authors");
            var author = authors.Find(x => x.Id == id).FirstOrDefault();

            if (author == null)
                throw new Exception("Author not found");
            
            return author;
        }

        public void Create(Author author)
        {
            author.Id = Guid.NewGuid().ToString();
            _database.GetCollection<Author>("Authors").InsertOne(author);
        }

        public int CountPublishedBlog(string authorId)
        {
            var author = Get(authorId);

            var blogs = _database.GetCollection<BlogPost>("BlogPosts");
            var blogByAuthor = blogs.Find(x => x.Author == author.Id).ToList();

            return blogByAuthor.Count();
        }
    }
}
