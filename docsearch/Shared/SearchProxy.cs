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

            var res = task.Result;

            return res;
        }

        /// <summary>
        /// this is a login function, intended to add more security to the search engine
        /// </summary>
        public bool LogIn()
        {
            Console.WriteLine("you must login first!");

            string connectionString = "Data Source=/Users/daddel/documents/skole/6sem/ark_principper/eksamensprojekt/users.db;";

            bool userLoginBool = false;

            // use a while loop so the user is continously prompted for login until they successfully login
            while (userLoginBool == false)
            {
                System.Console.WriteLine("username:");
                string typedUserName = Console.ReadLine();

                System.Console.WriteLine("password:");
                string typedPassword = Console.ReadLine();

                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();

                    // form sql query to select all users with the typed userName and password
                    string sql = "SELECT COUNT(*) FROM users WHERE user_name = @UserName AND password = @Password";

                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", typedUserName);
                        cmd.Parameters.AddWithValue("@Password", typedPassword);

                        // execute the above query
                        int result = Convert.ToInt32(cmd.ExecuteScalar());
                        
                        // if the result is larger than 0, it means a user has been found, we set the userLoginBool to true
                        if (result > 0)
                        {
                            Console.WriteLine("Login successful, you may proceed with the search.");

                            userLoginBool = true;
                        }

                        // else a user has not been found, we set the userLoginBool to false
                        else
                        {
                            Console.WriteLine("login failed, either username or password was incorrect!");
                            Console.WriteLine("please try again");

                            userLoginBool = false;
                        }
                    }
                }
            }

            if (userLoginBool == true)
            {
                return true;
            }

            else return false;
        }

    }
}

