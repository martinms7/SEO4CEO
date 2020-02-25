using System;
using System.Collections.Generic;
using System.Text;

namespace SEO4CEO_Core.DomainModels
{
    public class DomainResponse
    {
        public string ExpectedUri { get; set; }
        public List<int> MatchedPositions;
    }
}
