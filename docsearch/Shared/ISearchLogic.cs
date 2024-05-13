namespace Core
{
    public interface ISearchLogic
    {
        SearchResult Search(string[] query, int maxAmount);

        // public bool LogIn();
        public int LogIn();

        public int InsertUserSearch(int userId, string searchWord, int searchHits);

        public bool WebPageLogIn(string username, string password);
    }
}