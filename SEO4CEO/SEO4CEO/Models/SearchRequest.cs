using System.Collections.Generic;

namespace SEO4CEO.Models
{
    public class SearchRequest
    {
        public string Keywords { get; set; }
        public string ExpectedUri { get; set; }
        public List<int> MatchedPositions;
    }
}
