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
        public int LogIn()
        {
            Console.WriteLine("you must login first!");

            string connectionString = "Data Source=/Users/daddel/documents/skole/6sem/ark_principper/eksamensprojekt/userData.db;";

            bool userLoginBool = false;

            int userId = 0;

            // use a while loop so the user is continously prompted for login until they successfully login
            while (userLoginBool == false)
            {
                System.Console.WriteLine("username: - type exit to quit");
                string typedUserName = Console.ReadLine();

                if (typedUserName.Equals("exit")) Environment.Exit(0);

                System.Console.WriteLine("password: - type exit to quit");
                string typedPassword = Console.ReadLine();

                if (typedPassword.Equals("exit")) Environment.Exit(0);
                
                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();

                    // form sql query to select all users with the typed userName and password
                    string sql = "SELECT userId FROM users WHERE username = @UserName AND password = @Password";

                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", typedUserName);
                        cmd.Parameters.AddWithValue("@Password", typedPassword);

                        // execute the above query
                        userId = Convert.ToInt32(cmd.ExecuteScalar());
                        
                        // if the result is larger than 0, it means a user has been found, we set the userLoginBool to true
                        if (userId > 0)
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
                return userId;
            }

            else return 0;
        }

        /// <summary>
        /// Inserts the returned metadata from each userSearch into an SQLite database.
        /// </summary>
        public int InsertUserSearch(int userId, string searchWord, int searchHits)
        {
            string connectionString = "Data Source=/Users/daddel/documents/skole/6sem/ark_principper/eksamensprojekt/userData.db;";

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                string sql = "INSERT INTO userSearches (userId, searchWord, searchHits) VALUES (@userId, @searchWord, @searchHits); SELECT last_insert_rowid();";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@searchWord", searchWord);
                    cmd.Parameters.AddWithValue("@searchHits", searchHits);

                    int newRecordId = Convert.ToInt32(cmd.ExecuteScalar()); // Executes the query and returns the new record ID
                    Console.WriteLine($"Inserted new search record with ID: {newRecordId}");

                    return newRecordId;
                }
            }
        }



        public bool WebPageLogIn(string username, string password)
        {
            string connectionString = "Data Source=/Users/daddel/documents/skole/6sem/ark_principper/eksamensprojekt/userData.db;";

            bool userLoginBool = false;

            // use a while loop so the user is continously prompted for login until they successfully login
            
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                // form sql query to select all users with the typed userName and password
                string sql = "SELECT COUNT(*) FROM users WHERE username = @UserName AND password = @Password";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", username);
                    cmd.Parameters.AddWithValue("@Password", password);

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

            if (userLoginBool == true)
            {
                return true;
            }

            else return false;
        }

    }
}

