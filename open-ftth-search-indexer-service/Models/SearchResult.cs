using System.Collections.Generic;

namespace open_ftth_search_indexer_service.Models
{
    public class SearchResult
    {
        public List<RouteNode> nodes {get;set;}
        public List<Address> addresses{get;set;}

    }
}