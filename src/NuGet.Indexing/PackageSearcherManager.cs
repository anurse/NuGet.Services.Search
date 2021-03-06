﻿using Lucene.Net.Search;
using System;
using System.Collections.Generic;

namespace NuGet.Indexing
{
    public class PackageSearcherManager : SearcherManager
    {
        IDictionary<string, IDictionary<string, int>> _currentRankings;
        DateTime _rankingsTimeStampUtc;
        Rankings _rankings;

        public Guid Id { get; private set; }

        public PackageSearcherManager(Lucene.Net.Store.Directory directory, Rankings rankings)
            : base(directory)
        {
            _rankings = rankings;
        
            Id = Guid.NewGuid(); // Used for identifying changes to the searcher manager at runtime.
        }

        protected override void Warm(IndexSearcher searcher)
        {
            searcher.Search(new MatchAllDocsQuery(), 1);
        }

        public IDictionary<string, int> GetRankings(string context)
        {
            if (_currentRankings == null || (DateTime.Now - _rankingsTimeStampUtc) > TimeSpan.FromHours(24))
            {
                _currentRankings = _rankings.Load();
                _rankingsTimeStampUtc = DateTime.UtcNow;
            }

            IDictionary<string, int> rankings;
            if (_currentRankings.TryGetValue(context, out rankings))
            {
                return rankings;
            }

            return _currentRankings["Rank"];
        }
    }
}