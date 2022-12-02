using MongoDB.Bson.Serialization.Attributes;

namespace front_back.Database
{
    public class SecondThread
    {

        [BsonId]
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string comment { get; set; } = string.Empty;

    }
}
