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
        private readonly CommClass _crudeCommLayer;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _crudeCommLayer = new CommClass();
        }

        public IActionResult Index()
        {
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
                ExpectedUri = "www.infotrack.com.au"
            };
            var response = _crudeCommLayer.FindUriInSearch(request);
                
            return PartialView("_PositionsView", response);
        }

        public PartialViewResult SearchCustom(SearchRequest request)
        {
            var response = _crudeCommLayer.FindUriInSearch(request);

            return PartialView("_PositionsView", response);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
