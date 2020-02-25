using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace SEO4CEO_Core
{
    public class SearchService : ISearchService
    {
        public string FindUriInSearch(string keywords, string expectedUri, List<int> matchedPositions)
        {
            var client = new HttpClient();
            var testUri = new UriBuilder();
            testUri.Scheme = "https";
            testUri.Host = "google.com.au";
            testUri.Path = @"search";

            var queryPart = keywords.Replace(' ', '+');

            testUri.Query = $"num=100&q={queryPart}";

            var response = client.GetStringAsync(testUri.Uri);
            var responsePage = response.Result;
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
            sb.Append($"URI: {expectedUri} \t Positions: ");
            foreach (var result in resultLinks)
            {
                var resultIndex = resultLinks.IndexOf(result);
                if (resultIndex > 100)
                    break;
                //var searchResultLinkMatch = Regex.Match(anchorMatch, @"(<a href="" / url"));
                if (result.Contains(expectedUri))
                {
                    //resultPositions.Add(result, resultIndex);
                    matchedPositions.Add(resultIndex);
                    sb.Append($"{resultIndex},");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            //[A-Za-z0-9]+\.(com|org|net)

            return $"Keyword Search String:{keywords},Matching URL:{expectedUri}" +
                $"\n Sample Result Text:" +
                $"\n {sb} ";
        }
    }
}
