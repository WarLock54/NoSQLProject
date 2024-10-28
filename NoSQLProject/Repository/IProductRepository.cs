using MongoDB.Bson;
using NoSQLProject.Models;

namespace NoSQLProject.Repository
{
    public interface IProductRepository
    {// Create
        Task<ObjectId> Create(Product product);

        // Read
        Task<Product> Get(ObjectId objectId);
        Task<IEnumerable<Product>> fetchbyname(string name);

        // Update
        Task<bool> Update(ObjectId objectId, Product product);

        // Delete
        Task<bool> Delete(ObjectId objectId);
    }
}
