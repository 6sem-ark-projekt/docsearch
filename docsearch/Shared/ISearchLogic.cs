namespace Core
{
    public interface ISearchLogic
    {
        SearchResult Search(string[] query, int maxAmount);

        public bool LogIn(string user_name, string password);
    }
}