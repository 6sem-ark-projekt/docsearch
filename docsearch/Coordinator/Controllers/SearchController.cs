
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Coordinator.Service;
using Core;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Coordinator.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class CoordinatorController : ControllerBase
    {
        private readonly ILogger<CoordinatorController> _logger;


        private static CoordinatorService mService = new();


        public CoordinatorController(ILogger<CoordinatorController> logger)
        {
            _logger = logger;
        }

        [EnableCors("policy")]
        [HttpGet]
        [Route("{query}/{maxAmount}")]
        public SearchResult Get(string query, int maxAmount)
        {
            return mService.Get(query, maxAmount);
        }

        [HttpGet]
        [Route("ping")]
        public string? Ping()
        {
            return "Coordinator";
        }
    }
}

