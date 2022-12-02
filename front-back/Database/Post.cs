using MongoDB.Bson.Serialization.Attributes;

namespace front_back.Database
{
    public class Post
    {

        [BsonId]
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string PostName { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public string Public { get; set; } = string.Empty;

        public List<MainThread> Comments { get; set; } = new List<MainThread>();

    }
}
