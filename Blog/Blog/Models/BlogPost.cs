
using MongoDB.Bson.Serialization.Attributes;

namespace Blog.Models
{
    public class BlogPost
    {
        [BsonId]
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Author { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}