using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProduktMock.Models;

namespace ProduktMock.Controllers
{
    [Route("api/[Controller]")]
    public class MockController : ControllerBase
    {
        public MockController(AppDB db)
        {
            Db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetLatest()
        {
            await Db.Connection.OpenAsync();
            var query = new MockPostQuery(Db);
            var result = await query.LatestPostAsync();
            return new OkObjectResult(result);
        }

        //GET api/Mock/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActionResult(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new MockPostQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }
        
        //POST api/mock
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]MockPost body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }
        //PUT api/mock/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody]MockPost body)
        {
            await Db.Connection.OpenAsync();
            var query = new MockPostQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.CardNumber = body.CardNumber;
            result.isAccepted = body.isAccepted;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        //DELETE api/mock/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new MockPostQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }
        // DELETE api/blog
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await Db.Connection.OpenAsync();
            var query = new MockPostQuery(Db);
            await query.DeleteAllSync();
            return new OkResult();
        }
        public AppDB Db {get;}
    }
}