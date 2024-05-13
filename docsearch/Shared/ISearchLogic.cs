namespace Core
{
    public interface ISearchLogic
    {
        SearchResult Search(string[] query, int maxAmount);

        public bool LogIn();

        public bool WebPageLogIn(string username, string password);
    }
}