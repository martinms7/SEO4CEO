using System;
using System.Collections.Generic;
using System.Text;

namespace SEO4CEO_Core
{
    public interface ISearchService
    {
        string FindUriInSearch(string keywords, string expectedUri, List<int> matchedPositions);
    }
}
