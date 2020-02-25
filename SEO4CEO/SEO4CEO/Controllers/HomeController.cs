using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SEO4CEO.Models;
using SEO4CEO_Core;

namespace SEO4CEO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISearchService _searchService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _searchService = new SearchService();
        }

        public IActionResult Index()
        {
            var request = new SearchRequest()
            {
                Keywords = "Initial Words",
                ExpectedUri = "Initial URI",
                MatchedPositions = new List<int>()
            };
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public PartialViewResult SearchDefault()
        {
            var request = new SearchRequest()
            {
                Keywords = "online title search",
                ExpectedUri = "www.infotrack.com.au",
                MatchedPositions = new List<int>()
            };
            request.MatchedPositions = _searchService.FindUriInSearch(
                request.Keywords,
                request.ExpectedUri,
                request.MatchedPositions).ToList();
                
            return PartialView("_PositionsView", request);
            //return request;
        }

        public string SearchCustom(SearchRequest request)
        {
            //return FindUriInSearch(request.Keywords, request.ExpectedUri);
            return FindUriInSearch(request);
        }
        private string FindUriInSearch(SearchRequest request)
        {
            //https://www.google.com.au/search?num=100&q=online+title+search
            var client = new HttpClient();
            var testUri = new UriBuilder();
            testUri.Scheme = "https";
            testUri.Host = "google.com.au";
            testUri.Path = @"search";

            var queryPart = request.Keywords.Replace(' ', '+');

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
            sb.Append($"URI: {request.ExpectedUri} \t Positions: ");
            foreach (var result in resultLinks) 
            {
                var resultIndex = resultLinks.IndexOf(result);
                if (resultIndex > 100)
                    break;
                //var searchResultLinkMatch = Regex.Match(anchorMatch, @"(<a href="" / url"));
                if (result.Contains(request.ExpectedUri))
                {
                    //resultPositions.Add(result, resultIndex);
                    request.MatchedPositions.Add(resultIndex);
                    sb.Append($"{resultIndex},");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            //[A-Za-z0-9]+\.(com|org|net)

            return $"Keyword Search String:{request.Keywords},Matching URL:{request.ExpectedUri}" +
                $"\n Sample Result Text:" +
                $"\n {sb} ";
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
