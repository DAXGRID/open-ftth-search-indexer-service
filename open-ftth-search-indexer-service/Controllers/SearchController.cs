using System;
using Microsoft.AspNetCore.Mvc;
using open_ftth_search_indexer_service.Models;
using Typesense;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace openftth_search_indexer_service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private ITypesenseClient _client;
        private readonly ILogger<SearchController> _logger;


        public SearchController(ITypesenseClient client, ILogger<SearchController> logger)
        {
            _client = client;
            _logger = logger;
        }

        [HttpGet("{text}/{numOfResults}")]
        public async Task<ActionResult<List<SearchResults>>> GetAll(string text, string numOfResults)
        {
            var results = new List<SearchResults>();
            var nodesQuery = new SearchParameters
            {
                Text = text,
                QueryBy = "name",
                PerPage = numOfResults,
                TypoTokensThreshold = "0",
                NumberOfTypos = "0"
            };

            var nodeResult = await _client.Search<RouteNode>("RouteNodes", nodesQuery);

            foreach (var res in nodeResult.Hits)
            {
                results.Add(new SearchResults
                {
                    id = res.Document.id,
                    name = res.Document.name,
                    type = "RouteNodes",
                    coordinates = res.Document.coordinates
                });
            }

            var adressQuery = new SearchParameters
            {
                Text = text,
                QueryBy = "unitAddressDescription",
                PerPage = numOfResults
            };

            var addressResult = await _client.Search<Address>("Addresses", adressQuery);

            foreach(var res in addressResult.Hits)
            {
                results.Add(new SearchResults
                {
                    id = Guid.Parse(res.Document.id_lokalId),
                    name = res.Document.unitAddressDescription,
                    type = "Addresses",
                    coordinates = res.Document.position
                });
            }


            return results;


        }

    }
}