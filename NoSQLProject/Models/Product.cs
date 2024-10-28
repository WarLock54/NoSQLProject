using MongoDB.Bson;

namespace NoSQLProject.Models
{
    public class Product
    {
        public ObjectId ProductId { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
    }
}
