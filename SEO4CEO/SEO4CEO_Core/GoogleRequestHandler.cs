using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SEO4CEO_Core
{
    public class GoogleRequestHandler:IGoogleRequestHandler
    {
        public string GetGoogleResponse(string keywords)
        {
            var client = new HttpClient();
            var testUri = new UriBuilder();
            testUri.Scheme = "https";
            testUri.Host = "google.com.au";
            testUri.Path = @"search";

            var queryPart = keywords.Replace(' ', '+');

            testUri.Query = $"num=100&q={queryPart}";

            var response = client.GetStringAsync(testUri.Uri);
            return response.Result;
        }

        public string GetSearchResponse(string keywords)
        {
            return GetGoogleResponse(keywords);
        }
    }
}
