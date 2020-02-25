using System;
using System.Collections.Generic;
using System.Text;

namespace SEO4CEO_Core
{
    public interface ISearchRequestHandler
    {
        string GetSearchResponse(string keywords);
    }
}
