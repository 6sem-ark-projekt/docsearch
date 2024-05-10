using System;
namespace Core
{
    public class Paths
    {
        public static string FOLDER = @"/Users/daddel/documents/skole/6sem/ark_principper/seData/medium";

        public static string GetDatabase(int id)
        {
            return id == 1 ? "/Users/daddel/documents/skole/6sem/ark_principper/eksamensprojekt/searchDBm1.db" :
                             "/Users/daddel/documents/skole/6sem/ark_principper/eksamensprojekt/searchDBm2.db";

        }
    }
}
