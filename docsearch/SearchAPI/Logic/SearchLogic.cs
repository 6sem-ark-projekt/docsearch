﻿using System;
using System.Collections.Generic;
using Core;
using Core.Model;

namespace SearchAPI.Logic
{
    public class SearchLogic : ISearchLogic
    {
        public int LogIn()
        {
            return 0;
        }

        public int InsertUserSearch(int userId, string searchWord, int searchHits)
        {
            return 0;
        }

        public bool WebPageLogIn(string username, string password)
        {
            return true;
        }

        IDatabase mDatabase;

        Dictionary<string, int> mWords;

        public SearchLogic(IDatabase database)
        {
            mDatabase = database;
        }

        /* Perform search of documents containing words from query. The result will
         * contain details about amost maxAmount of documents.
         */
        public SearchResult Search(String[] query, int maxAmount)
        {
            List<string> ignored;

            DateTime start = DateTime.Now;

            int sleep = int.Parse(Environment.GetEnvironmentVariable("Sleep"));
            Thread.Sleep(sleep);

            // Convert words to wordids
            var wordIds = mDatabase.GetWordIds(query, out ignored);

            // perform the search - get all docIds
            var docIds = mDatabase.GetDocuments(wordIds);

            // get ids for the first maxAmount             
            var top = new List<int>();
            foreach (var p in docIds.GetRange(0, Math.Min(maxAmount, docIds.Count)))
                top.Add(p.Key);

            // compose the result.
            // all the documentHit
            List<DocumentHit> docresult = new List<DocumentHit>();
            int idx = 0;
            foreach (var doc in mDatabase.GetDocDetails(top))
            {
                var missing = mDatabase.WordsFromIds(mDatabase.getMissing(doc.mId, wordIds));

                docresult.Add(new DocumentHit
                {
                    Document = doc,
                    NoOfHits = docIds[idx++].Value,
                    Missing = missing
                });
            }

            return new SearchResult
            {
                Query = query,
                Hits = docIds.Count,
                DocumentHits = docresult,
                Ignored = ignored,
                TimeUsed = DateTime.Now - start
            };
        }
    }
}
