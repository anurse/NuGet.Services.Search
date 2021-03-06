﻿using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using System;
using System.Collections.Generic;

namespace NuGet.Indexing
{
    public class PackageAnalyzer : PerFieldAnalyzerWrapper
    {
        public PackageAnalyzer()
            : base(new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), CreateFieldAnalyzers())
        {
        }

        private static IDictionary<string, Analyzer> CreateFieldAnalyzers()
        {
            return new Dictionary<string, Analyzer>(StringComparer.OrdinalIgnoreCase)
            {
                { "Id", new IdentifierKeywordAnalyzer() },
                { "TokenizedId", new IdentifierAnalyzer() },
                { "ShingledId", new ShingledIdentifierAnalyzer() },
                { "Version", new VersionAnalyzer() },
                { "Title", new DescriptionAnalyzer() },
                { "Description", new DescriptionAnalyzer() },
                { "Authors", new DescriptionAnalyzer() },
                { "Owners", new DescriptionAnalyzer() },
                { "Tags", new TagsAnalyzer() }
            };
        }
    }
}
