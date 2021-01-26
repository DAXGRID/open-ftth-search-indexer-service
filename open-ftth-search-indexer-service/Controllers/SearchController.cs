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
        public async Task<ActionResult<SearchResult>> Get(string text, string numOfResults)
        {

            var results = new SearchResult
            {
                nodes = SearchNodes(text,numOfResults).Result,
                addresses = SearchAddress(text,numOfResults).Result
            };
            
            return results;

        }

        private async Task<List<RouteNode>> SearchNodes(string text, string numOfResults)
        {
            List<RouteNode> nodes = new List<RouteNode>();
            var query = new SearchParameters
            {
                Text = text,
                QueryBy = "name",
                PerPage = numOfResults
            };

            var nodeResult = await _client.Search<RouteNode>("RouteNodes", query);
            foreach (var res in nodeResult.Hits)
            {
                nodes.Add(new RouteNode
                {
                    id = res.Document.id,
                    name = res.Document.name,
                    incrementalId = res.Document.incrementalId
                });
            }

            return nodes;
        }

        private async Task<List<Address>> SearchAddress(string text,string numOfResults)
        {
            List<Address> addresses = new List<Address>();
            var adressQuery = new SearchParameters
            {
                Text = text,
                QueryBy = "accessAddressDescription",
                PerPage = numOfResults
            };

            var addressResult = await _client.Search<Address>("Addresses", adressQuery);
            foreach (var res in addressResult.Hits)
            {
                addresses.Add(new Address
                {
                    id_lokalId = res.Document.id_lokalId,
                    door = res.Document.door,
                    floor = res.Document.floor,
                    unitAddressDescription = res.Document.unitAddressDescription,
                    houseNumberId = res.Document.houseNumberId,
                    status = res.Document.status,
                    houseNumberDirection = res.Document.houseNumberDirection,
                    houseNumberText = res.Document.houseNumberText,
                    position = res.Document.position,
                    accessAddressDescription = res.Document.accessAddressDescription
                });
            }

            return addresses;
        }
    }
}