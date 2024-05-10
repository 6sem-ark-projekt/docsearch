using Core;
using Core.Model;

namespace Coordinator.Service
{
    public class CoordinatorService
    {
        private static readonly string[] _servers = { "https://localhost:7132", "https://localhost:7133" };

        private HttpClient _client = new HttpClient();

        public CoordinatorService()
        {
        }

        public SearchResult Get(string query, int maxAmount)
        {
            
            DateTime start = DateTime.Now;

            List<Task<SearchResult>> tasks = new();

            // start and remember all slaves
            foreach (var serverUrl in _servers)
            {

                var completeUrl = $"{serverUrl}/api/search/{query}/{maxAmount}";

                Console.WriteLine($"Coordinator request {completeUrl}");

                tasks.Add(GetSearchTask(completeUrl));

            }
            // the result will start as empty
            var result = new SearchResult
            {
                Query = query.Split(","),
                Hits = 0,
                DocumentHits = new List<DocumentHit>(),
                Ignored = new List<string>(),
                TimeUsed = TimeSpan.FromMilliseconds(0)
            };

            //Merge result from all slaves
            foreach (var slave in tasks)
            {
                var partialResult = slave.Result;

                MergeResult(result, partialResult);
            }

            //Sort result decreasingly 
            result.DocumentHits = result.DocumentHits.OrderByDescending(hit => hit.NoOfHits).ToList();

            result.TimeUsed = DateTime.Now - start;
            return result;
            //return JsonConvert.SerializeObject(result, Formatting.Indented);

        }

        private void MergeResult(SearchResult result, SearchResult partialResult)
        {
            result.Hits += partialResult.Hits;
            result.DocumentHits.AddRange(partialResult.DocumentHits);
            // Add ignored
            foreach (var ig in partialResult.Ignored)
                if (!result.Ignored.Contains(ig))
                    result.Ignored.Add(ig);
        }

        private Task<SearchResult> GetSearchTask(string url)
        {
            var res = new Task<SearchResult>(() => Search(url));
            res.Start();
            return res;
        }

        private SearchResult Search(string url)
        {
            var result = _client.GetFromJsonAsync<SearchResult>(url).Result;

            return result;
        }
    }
}

