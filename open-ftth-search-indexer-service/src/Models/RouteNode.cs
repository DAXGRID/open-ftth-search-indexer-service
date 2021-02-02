using System;
namespace open_ftth_search_indexer_service.Models
{
    public class RouteNode
    {
        public Guid id {get;set;}
        public int  incrementalId {get;set;}
        public string name {get;set;}

        public string coordinates {get;set;}
    }
}
