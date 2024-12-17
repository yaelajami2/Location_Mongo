using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WebApplication14.Models
{
    public class Log_user
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("location")]
        public string location { get; set; }
        [BsonElement("Date")]
        public string Date { get; set; }
        [BsonElement("ScreenInfo")]
        public string ScreenInfo { get; set; }

    }
}
