using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO4CEO.Models
{
    public class QueryResult
    {
        public int Hits { get; set; }
        public int TopPosition { get; set; }
        public int DateTimeUtc { get; set; }
    }
}
