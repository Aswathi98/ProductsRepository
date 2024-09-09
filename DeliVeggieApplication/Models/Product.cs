using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeliVeggieApplication.Models
{
    // Models/Product.cs
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public DateTime EntryDate { get; set; }
    }

}
