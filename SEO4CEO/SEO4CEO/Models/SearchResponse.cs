using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO4CEO.Models
{
    public class SearchResponse
    {
        public string ExpectedUri { get; set; }
        public List<int> MatchedPositions;
    }
}
