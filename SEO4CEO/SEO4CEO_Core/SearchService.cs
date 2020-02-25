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

        public SearchService():this(new GoogleRequestHandler())
        {

        }
        public SearchService(ISearchRequestHandler requestHandler)
        {
            _requestHandler = requestHandler;
        }
        public DomainResponse FindUriInSearch(DomainRequest request)
        {
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

            //var resultPositions = new Dictionary<string, int>();

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
                //var searchResultLinkMatch = Regex.Match(anchorMatch, @"(<a href="" / url"));
                if (result.Contains(request.ExpectedUri))
                {
                    //resultPositions.Add(result, resultIndex);
                    response.MatchedPositions.Add(resultIndex);
                    sb.Append($"{resultIndex},");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            //[A-Za-z0-9]+\.(com|org|net)

            //return $"Keyword Search String:{keywords},Matching URL:{expectedUri}" +
            //    $"\n Sample Result Text:" +
            //    $"\n {sb} ";
            return response;
        }
    }
}
