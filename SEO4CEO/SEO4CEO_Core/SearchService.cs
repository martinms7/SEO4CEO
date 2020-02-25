using log4net;
using SEO4CEO_Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace SEO4CEO_Core
{
    public class SearchService : ISearchService
    {
        private ISearchRequestHandler _requestHandler;
        private readonly static ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public SearchService():this(new GoogleRequestHandler())
        {

        }
        public SearchService(ISearchRequestHandler requestHandler)
        {
            _requestHandler = requestHandler;
        }

        public DomainResponse FindUriInSearch(DomainRequest request)
        {
            if(request == null)
            {
                var message = "Missing Request object";
                _log.Error(message);

                throw new ArgumentNullException("request", message);
            }

            var responsePage = _requestHandler.GetSearchResponse(request.Keywords);
            var anchorMatches = Regex.Matches(responsePage, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);

            var resultLinks = new List<string>();
            foreach (Match anchorMatch in anchorMatches)
            {
                var matchValue = anchorMatch.Groups[1].Value;

                if (matchValue.StartsWith(@"<a href=""/url"))
                {
                    resultLinks.Add(matchValue);
                }
            }

            var sb = new StringBuilder();
            sb.Append($"URI: {request.ExpectedUri} \t Positions: ");
            var response = new DomainResponse()
            {
                ExpectedUri = request.ExpectedUri,
                MatchedPositions = new List<int>()
            };

            foreach (var result in resultLinks)
            {
                var resultIndex = resultLinks.IndexOf(result);
                if (resultIndex > 100)
                    break;

                if (result.Contains(request.ExpectedUri))
                {
                    response.MatchedPositions.Add(resultIndex);
                }
            }

            return response;
        }
    }
}
