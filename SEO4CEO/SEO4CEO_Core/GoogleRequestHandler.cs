using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using log4net;
using log4net.Core;

namespace SEO4CEO_Core
{
    public class GoogleRequestHandler:IGoogleRequestHandler
    {
        private readonly static ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string GetGoogleResponse(string keywords)
        {
            var client = new HttpClient();
            var requestUri = new UriBuilder();
            requestUri.Scheme = "https";
            requestUri.Host = "google.com.au";
            requestUri.Path = @"search";

            var queryPart = !string.IsNullOrEmpty(keywords)? keywords.Replace(' ', '+') : string.Empty;

            requestUri.Query = $"num=100&q={queryPart}";

            var response = client.GetStringAsync(requestUri.Uri);
            return response.Result;
        }

        public string GetSearchResponse(string keywords)
        {
            return GetGoogleResponse(keywords);
        }
    }
}
