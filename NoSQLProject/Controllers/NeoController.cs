using Microsoft.AspNetCore.Mvc;
using NoSQLProject.Models;
using NoSQLProject.Repository;

namespace NoSQLProject.Controllers
{
    public class NeoController : Controller
    {
        private readonly INeoRepository neoRepository;
        public NeoController(INeoRepository _neoRepository)
        {
            this.neoRepository = _neoRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Movie movie)
        {
           neoRepository.Create(movie);
            return Ok();
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            neoRepository.fetchbyname(name);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            neoRepository.Get();
            return Ok();
        }
        [HttpDelete("({id})")]
        public async Task<IActionResult> Delete(int Id)
        {
            neoRepository.Delete(Id);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Movie movie)
        {
            neoRepository.Update(id, movie);
            return new JsonResult($"{id}");
        }
        [HttpGet("{eid}/assignperson/{did}/")]
        public async Task<IActionResult> AssignMovie(int did,int eid)
        {
             neoRepository.Assign(did, eid);
            return Ok();
        }

    }
}
