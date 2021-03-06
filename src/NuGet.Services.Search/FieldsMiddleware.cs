﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using NuGet.Indexing;

namespace NuGet.Services.Search
{
    public class FieldsMiddleware : SearchMiddleware
    {
        public FieldsMiddleware(OwinMiddleware next, string path, Func<PackageSearcherManager> searcherManagerThunk) : base(next, path, searcherManagerThunk) { }

        protected override async Task Execute(IOwinContext context)
        {
            Trace.TraceInformation("Fields");

            await WriteResponse(context, IndexAnalyzer.GetDistinctStoredFieldNames(SearcherManager));
        }
    }
}
