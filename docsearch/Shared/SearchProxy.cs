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

        public bool LogIn()
        {
            Console.WriteLine("you must login first!");

            string connectionString = "Data Source=/Users/daddel/documents/skole/6sem/ark_principper/eksamensprojekt/users.db;";

            bool user_login_bool = false;

            while (user_login_bool == false)
            {
                System.Console.WriteLine("user_name:");
                string typedUserName = Console.ReadLine();

                System.Console.WriteLine("password:");
                string typedPassword = Console.ReadLine();

                System.Console.WriteLine($"user_name: {typedUserName}, password: {typedPassword}");

                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT COUNT(*) FROM users WHERE user_name = @UserName AND password = @Password";

                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", typedUserName);
                        cmd.Parameters.AddWithValue("@Password", typedPassword);

                        int result = Convert.ToInt32(cmd.ExecuteScalar());

                        if (result > 0)
                        {
                            Console.WriteLine("Login successful, you may proceed with the search.");

                            user_login_bool = true;
                        }

                        else
                        {
                            Console.WriteLine("login failed, either user_name or password was incorrect!");
                            Console.WriteLine("please try again");

                            user_login_bool = false;
                        }
                    }
                }
            }

            if (user_login_bool == true)
            {
                return true;
            }

            else return false;
        }

    }
}

