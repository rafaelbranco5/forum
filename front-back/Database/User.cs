using MongoDB.Bson.Serialization.Attributes;

namespace front_back.Database
{
    public class User
    {

        [BsonId]
        public int Id { get; set; } = 0;
        
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
