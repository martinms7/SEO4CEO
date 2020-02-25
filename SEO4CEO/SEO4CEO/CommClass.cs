using SEO4CEO.Models;
using SEO4CEO_Core;
using SEO4CEO_Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO4CEO
{
    public class CommClass
    {
        private readonly ISearchService _searchService;
        public CommClass():this(new SearchService())
        {

        }
        public CommClass(ISearchService service)
        {
            _searchService = service;
        }

        public SearchResponse FindUriInSearch(SearchRequest request)
        {

            var domainRequest = Map(request);
            return Map(_searchService.FindUriInSearch(domainRequest));
        }

        private DomainRequest Map(SearchRequest request)
        {
            return new DomainRequest()
            {
                Keywords = request.Keywords,
                ExpectedUri = request.ExpectedUri
            };
        }

        private SearchResponse Map(DomainResponse response)
        {
            return new SearchResponse()
            {
                ExpectedUri = response.ExpectedUri,
                MatchedPositions = response.MatchedPositions
            };
        }
    }
}
