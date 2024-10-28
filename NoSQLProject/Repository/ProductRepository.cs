using MongoDB.Bson;
using MongoDB.Driver;
using NoSQLProject.Models;
using System.Xml.Linq;

namespace NoSQLProject.Repository
{
   
    public class ProductRepository : IProductRepository
    {

        private readonly IMongoCollection<Product> _product;
        public ProductRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("ProductDB");
            var collection = database.GetCollection<Product>(nameof(Product));
            _product = collection;
        }
        async Task<ObjectId> IProductRepository.Create(Product product)
        {
            await _product.InsertOneAsync(product);
            return product.ProductId;
        }

        async Task<bool> IProductRepository.Delete(ObjectId objectId)
        {
            var filter=Builders<Product>.Filter.Eq(t=>t.ProductId,objectId);
            var result=await _product.DeleteOneAsync(filter);
            return result.DeletedCount==1;
        }
        async Task<IEnumerable<Product>> IProductRepository.fetchbyname(string name)
        {
            var filter=Builders<Product>.Filter.Eq(t=>t.Name,name);
            var result=await _product.Find(filter).ToListAsync();
            return result;
        }

        async Task<Product> IProductRepository.Get(ObjectId objectId)
        {
            var filter=Builders<Product>.Filter.Eq(t=>t.ProductId,objectId);
            var result = await _product.Find(filter).FirstOrDefaultAsync();
            return result;
        }

        async Task<bool> IProductRepository.Update(ObjectId objectId, Product product)
        {
            var filter = Builders<Product>.Filter.Eq(t => t.ProductId, objectId);
            var oldresult =  Builders<Product>.Update.Set(t=>t.Name,product.Name).Set(t=>t.Price,product.Price);
            var result=await _product.UpdateOneAsync(filter,oldresult);
            return result.ModifiedCount==1;
        }
    }
}
