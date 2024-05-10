using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Shared;
using static System.Net.WebRequestMethods;
using Microsoft.Data.Sqlite;
using System.Reflection.Metadata;

namespace Core
{
    public class SearchProxy : ISearchLogic
    {
        private string serverEndPoint = "http://localhost:5148/api/search/";

        private HttpClient mHttp;

        public SearchProxy()
        {
            mHttp = new System.Net.Http.HttpClient();
        }

        public SearchResult Search(string[] query, int maxAmount)
        {
            var task = mHttp.GetFromJsonAsync<SearchResult>($"{serverEndPoint}{String.Join(",", query)}/{maxAmount}");
            //var resultStr = response.Content.ReadAsStringAsync().Result;
            var res = task.Result;
            // result = JsonSerializer.Deserialize<SearchResult>(resultStr);


            System.Console.WriteLine($"printing server end point: {serverEndPoint}");


            return res;
        }

        public bool LogIn(string userName, string password)
        {
            string connectionString = "Data Source=/Users/daddel/documents/skole/6sem/ark_principper/eksamensprojekt/users.db;";

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT COUNT(*) FROM users WHERE user_name = @UserName AND password = @Password";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@Password", password);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());

                    if (result > 0)
                    {
                        return true;
                    }

                    else
                    {
                        return false;
                    }

                }
            }
        }

    }
}

