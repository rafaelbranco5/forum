using MongoDB.Bson.Serialization.Attributes;

namespace front_back.Database
{
    public class MainThread
    {

        [BsonId]
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;
        
        public string Comment { get; set; } = string.Empty;

        public List<SecondThread> Replies { get; set; } = new List<SecondThread>();
    }
}
