using Microsoft.AspNetCore.Mvc;
using Core;

namespace SearchAPI.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        [Route("{query}/{maxAmount}")]
        public SearchResult Search(string query, int maxAmount)
        {
            int id = int.Parse(Environment.GetEnvironmentVariable("Id"));
            var logic = SearchAPI.Logic.SearchFactory.GetSearchLogic(id);
            
            return logic.Search(query.Split(","), maxAmount);
            
        }

        [HttpGet]
        [Route("ping")]
        public string? Ping()
        {
            return Environment.GetEnvironmentVariable("Name");
        }


    }
}

