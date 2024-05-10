using System;
using System.Collections.Generic;
using Core;

namespace ConsoleSearch
{
    public class App
    {
        public App()
        {
        }

        public void Run()
        {
            ISearchLogic mSearchLogic = SearchFactory.GetProxy();

            Console.WriteLine("Console Search");

            Console.WriteLine("you must login first!");

            bool user_login_bool = false;

            while (user_login_bool == false)
            {
                System.Console.WriteLine("user_name:");
                string user_name = Console.ReadLine();

                System.Console.WriteLine("password:");
                string password = Console.ReadLine();

                System.Console.WriteLine($"you entered, user_name: {user_name}, password: {password}");

                user_login_bool = mSearchLogic.LogIn(user_name, password);

                if (!user_login_bool)
                {
                    Console.WriteLine("login failed, either user_name or password was incorrect!");
                    Console.WriteLine("please try again");
                }
            }

            Console.WriteLine("Login successful, you may proceed with the search.");

            while (true)
            {

                Console.WriteLine("enter search terms - q for quit");
                string input = Console.ReadLine();
                if (input.Equals("q")) break;

                var query = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                var result = mSearchLogic.Search(query, 10);

                if (result.Ignored.Count > 0)
                {
                    Console.WriteLine($"Ignored: {string.Join(',', result.Ignored)}");
                }

                int idx = 1;
                foreach (var doc in result.DocumentHits)
                {
                    Console.WriteLine($"{idx} : {doc.Document.mUrl} -- contains {doc.NoOfHits} search terms");
                    Console.WriteLine("Index time: " + doc.Document.mIdxTime);
                    Console.WriteLine($"Missing: {ArrayAsString(doc.Missing.ToArray())}");
                    idx++;
                }
                Console.WriteLine("Documents: " + result.Hits + ". Time: " + result.TimeUsed.TotalMilliseconds);
            }
        }

        string ArrayAsString(string[] s)
        {
            return s.Length == 0 ? "[]" : $"[{String.Join(',', s)}]";
            //foreach (var str in s)
            //    res += str + ", ";
            //return res.Substring(0, res.Length - 2) + "]";
        }
    }
}
