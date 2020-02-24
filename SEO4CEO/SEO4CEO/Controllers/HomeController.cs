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

namespace SEO4CEO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public string SearchDefault()
        {
            return FindUriInSearch("online title search", "propertyshark.com");
        }

        public string SearchCustom(SearchRequest request)
        {
            return FindUriInSearch(request.Keywords, request.ExpectedUri);
        }
        private string FindUriInSearch(string keywords, string uri)
        {
            //https://www.google.com.au/search?num=100&q=online+title+search
            var client = new HttpClient();
            var testUri = new UriBuilder();
            testUri.Scheme = "https";
            testUri.Host = "google.com.au";
            testUri.Path = @"search";
            testUri.Query = "num=100&q=online+title+search";

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

            var resultPositions = new Dictionary<string, int>();
            var sb = new StringBuilder();
            sb.Append($"URI: {uri} \t Positions: ");
            foreach (var result in resultLinks) 
            {
                var resultIndex = resultLinks.IndexOf(result);
                if (resultIndex > 100)
                    break;
                //var searchResultLinkMatch = Regex.Match(anchorMatch, @"(<a href="" / url"));
                if (result.Contains(uri))
                {
                    resultPositions.Add(result, resultIndex);

                    sb.Append($"{resultIndex},");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            //[A-Za-z0-9]+\.(com|org|net)

            return $"Test Action String:{keywords},URL:{uri}" +
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
