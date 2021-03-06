﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Index;
using Microsoft.Owin;
using NuGet.Indexing;

namespace NuGet.Services.Search
{
    public class RangeMiddleware : SearchMiddleware
    {
        public RangeMiddleware(OwinMiddleware next, string path, Func<PackageSearcherManager> searcherManagerThunk) : base(next, path, searcherManagerThunk) { }

        protected override async Task Execute(IOwinContext context)
        {
            Trace.TraceInformation("Range: {0}", context.Request.QueryString);

            string min = context.Request.Query["min"];
            string max = context.Request.Query["max"];

            string content = "[]";

            int minKey;
            int maxKey;
            if (min != null && max != null && int.TryParse(min, out minKey) && int.TryParse(max, out maxKey))
            {
                Trace.TraceInformation("Searcher.KeyRangeQuery(..., {0}, {1})", minKey, maxKey);

                content = Searcher.KeyRangeQuery(SearcherManager, minKey, maxKey);
            }

            await WriteResponse(context, content);
        }
    }
}
