using NUnit.Framework;
using NSubstitute;
using SEO4CEO_Core;
using SEO4CEO_Core.DomainModels;
using System;
using System.Collections.Generic;

namespace SEO4CEO_CoreTests
{
    [TestFixture]
    public class SearchServiceTests
    {
        private SearchService _service;
        private ISearchRequestHandler _searchRequestHandler;

        private const string RAW = "<div class=\"kCrYT\"><a href=\"/url? q = https://www.biggerpockets.com/forums/70/topics/306666-online-title-search-company&amp;sa=U&amp;ved=2ahUKEwjoxqS8n-nnAhWzuXEKHQpYAkwQFjACegQIZBAB&amp;usg=AOvVaw0zDWT6ziMV-pYD2OniHaPf\"><div class=\"BNeawe vvjwJb AP7Wnd\">Online Title search company - BiggerPockets</div><div class=\"BNeawe UPmit AP7Wnd\">https://www.biggerpockets.com &#8250; forums &#8250; topics &#8250; 306666-online-title-sea...</div></a></div>";

        [SetUp]
        public void Setup()
        {
            var testResponse = new DomainResponse()
            {
                ExpectedUri = "biggerpockets.com",
                MatchedPositions = new List<int>()
            };

            _searchRequestHandler = Substitute.For<ISearchRequestHandler>();
            _searchRequestHandler.GetSearchResponse(Arg.Any<string>())
                .Returns(RAW);


        }

        [Test]
        public void FindUriInSearch_Parse_Success()
        {
            _service = new SearchService(_searchRequestHandler);

            var testRequest = new DomainRequest()
            {
                Keywords = "online title search",
                ExpectedUri = "biggerpockets.com"
            };

            var response = _service.FindUriInSearch(testRequest);

            _searchRequestHandler.Received(1).GetSearchResponse(testRequest.Keywords);
            Assert.That(response.ExpectedUri, Is.EqualTo(testRequest.ExpectedUri));
            Assert.That(response.MatchedPositions.Count, Is.EqualTo(1));


            Assert.Pass();
        }
        
        [Test]
        public void FindUriInSearch_NullRequest_Throws()
        {
            _service = new SearchService(_searchRequestHandler);

            Assert.Throws<ArgumentNullException>(()=>_service.FindUriInSearch(null));
            _searchRequestHandler.Received(0).GetSearchResponse(Arg.Any<string>());

        }
    }
}