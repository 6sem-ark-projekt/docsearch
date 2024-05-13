using System;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace LoadBalancer.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class LoadBalancerController : ControllerBase
    {
        private static readonly string[] _servers = {
            "https://localhost:7132/api/search",
            "https://localhost:7133/api/search"
        };

        private static int next = 0;
        private static readonly HttpClient client = new HttpClient();

        [HttpGet]
        [Route("{query}/{maxAmount}")]
        public async Task<IActionResult> Get(string query, int maxAmount)
        {
            int originalNext = next;
            bool serverAvailable = await IsServerAvailable(_servers[next]);

            int attempts = 0;
            while (!serverAvailable && attempts < _servers.Length)
            {
                next = (next + 1) % _servers.Length;
                if (next == originalNext) // Completed a full cycle
                {
                    return StatusCode(503, "No servers available to handle the request.");
                }
                serverAvailable = await IsServerAvailable(_servers[next]);
                attempts++;
            }

            string server = $"{_servers[next]}/{query}/{maxAmount}";
            next = (next + 1) % _servers.Length; // Update for the next request

            System.Console.WriteLine("Redirecting to server: " + server);
            return Redirect(server);
        }

        [HttpGet]
        [Route("ping")]
        public IActionResult Ping()
        {
            return Ok(Environment.GetEnvironmentVariable("id") ?? "No ID set for this service");
        }

        private async Task<bool> IsServerAvailable(string serverUrl)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(serverUrl + "/ping");
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false; // Server is down or unreachable
            }
        }
    }
}
