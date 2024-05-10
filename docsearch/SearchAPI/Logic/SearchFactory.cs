using System;
using Core;
namespace SearchAPI.Logic
{
    public class SearchFactory
    {

        public static ISearchLogic GetSearchLogic(int id)
        {
            return new SearchLogic(new Database(id));
        }
    }
}

