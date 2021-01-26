using System;
using Microsoft.AspNetCore.Mvc;
using openftth_search_indexer_service.Models;
using Typesense;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

namespace openftth_search_indexer_service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RouteNodesController : ControllerBase
    {
        private ITypesenseClient _client;

        public RouteNodesController(ITypesenseClient client)
        {
            _client = client;
        }
        [HttpGet("{name}/{field}")]
        public async Task<ActionResult<List<RouteNode>>> Get(string name,string field)
        {
            RouteNode node = new RouteNode();
            List<RouteNode> nodes = new List<RouteNode>();
            var query = new SearchParameters
            {
                Text = name,
                QueryBy = field
            };

            var result = await _client.Search<RouteNode>("RouteNodes", query);
            foreach (var res in result.Hits)
            {
                nodes.Add(node = new RouteNode
                {
                    id = res.Document.id,
                    name = res.Document.name,
                    incrementalId = res.Document.incrementalId
                });
            }

            return nodes;

        }
    }
}