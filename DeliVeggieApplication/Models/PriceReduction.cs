using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace DeliVeggieApplication.Models
{
    public class PriceReduction
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int DayOfWeek { get; set; } // 0 for Sunday, 1 for Monday, etc.
        public double Reduction { get; set; }
    }
}