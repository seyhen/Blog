using MongoDB.Bson.Serialization.Attributes;

namespace Blog.Models
{
    public class Author
    {
        [BsonId]
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}
