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
    public class SegmentsMiddleware : SearchMiddleware
    {
        public SegmentsMiddleware(OwinMiddleware next, string path, Func<PackageSearcherManager> searcherManagerThunk) : base(next, path, searcherManagerThunk) { }

        protected override async Task Execute(IOwinContext context)
        {
            Trace.TraceInformation("Segments");

            await WriteResponse(context, IndexAnalyzer.GetSegments(SearcherManager));
        }
    }
}
