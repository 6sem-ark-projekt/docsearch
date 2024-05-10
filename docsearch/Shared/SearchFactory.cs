using System;
using Shared;
namespace Core
{
    public class SearchFactory
    {
        public static ISearchLogic GetProxy() {
            System.Console.WriteLine("inside the public class SearchFactory");

            return new SearchProxy();
        }
    }
}

