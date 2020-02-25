using SEO4CEO_Core.DomainModels;
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
        /// <param name="request">A search request</param>
        /// <returns>A response containing a collection of integers representing where expectedUri was found</returns>
        DomainResponse FindUriInSearch(DomainRequest request);
    }
}