using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CommClass> _logger;

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
            try
            {
                var domainRequest = Map(request);
                return Map(_searchService.FindUriInSearch(domainRequest));
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex,"Returned empty response. Not the best but you'll forgive me right?");
                return new SearchResponse() {
                    ExpectedUri = request.ExpectedUri,
                    MatchedPositions = new List<int>() };
            }
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
                MatchedPositions = response.MatchedPositions,
                QueryResults = MapQueryResults(response)
            };
        }

        private List<QueryResult> MapQueryResults(DomainResponse response)
        {
            var queryResults = new List<QueryResult>();
            foreach(var result in response.SeoResults)
            {
                queryResults.Add(new QueryResult()
                {
                    Hits = result.Hits,
                    TopPosition = result.TopPosition,
                    DateTimeUtc = result.DateTimeUtc
                });
            }
            return queryResults;
        }
    }
}
