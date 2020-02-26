using System;
using System.Collections.Generic;
using System.Text;

namespace SEO4CEO_Data
{
    public interface ISqlRepository
    {
        bool InsertSearchResults(int hits, int topPosition);
        IEnumerable<SeoResult> RetrieveTopResults();
    }
}
