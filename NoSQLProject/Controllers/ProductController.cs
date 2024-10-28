using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NoSQLProject.Models;
using NoSQLProject.Repository;

namespace NoSQLProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        [HttpGet("get/id")]
        public  async Task<IActionResult> Get(string Id)
        {
            var product = await productRepository.Get(ObjectId.Parse(Id));
            return new JsonResult(product);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            var id = await productRepository.Create(product);
            return new JsonResult(id.ToString());
        }
        [HttpPut("id")]
        public async Task<IActionResult> Update(string id,Product product)
        {
            var result = await productRepository.Update(ObjectId.Parse(id), product);
            return new JsonResult($"{result.ToString()}");
        }
        [HttpGet("ByName/Name")]
        public async Task<IActionResult> GetByProductName(string name)
        {
            var result =await productRepository.fetchbyname(name);
            return new JsonResult(result.ToString());
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(string id)
        {
            var result=await productRepository.Delete(ObjectId.Parse(id));
            return new JsonResult(result.ToString());
        }
    }
}
