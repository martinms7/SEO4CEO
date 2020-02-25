using System;
using System.Collections.Generic;
using System.Text;

namespace SEO4CEO_Core
{
    public interface ISearchService
    {
        /// <summary>
        /// Locates all instances of the expected uri string in the first 100 results of search
        /// </summary>
        /// <param name="keywords">search terms as single</param>
        /// <param name="expectedUri">Uri to search for in results</param>
        /// <param name="matchedPositions">A collection of integers representing where expectedUri was found</param>
        /// <returns>A collection of integers representing where expectedUri was found</returns>
        IEnumerable<int> FindUriInSearch(string keywords, string expectedUri, List<int> matchedPositions);
    }
}
