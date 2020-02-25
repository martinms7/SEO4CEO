using System;
using System.Collections.Generic;
using System.Text;

namespace SEO4CEO_Core
{
    public interface IGoogleRequestHandler : ISearchRequestHandler
    {
        string GetGoogleResponse(string keywords);
    }
}
