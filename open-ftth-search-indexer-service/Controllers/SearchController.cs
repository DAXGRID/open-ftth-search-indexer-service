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
        [HttpGet("{nodeText}/{addressText}")]
        public async Task<ActionResult<SearchResult>> Get(string nodeText, string addressText)
        {

            var results = new SearchResult
            {
                nodes = SearchNodes(nodeText).Result,
                addresses = SearchAddress(addressText).Result
            };

            return results;

        }

        private async Task<List<RouteNode>> SearchNodes(string text)
        {
            List<RouteNode> nodes = new List<RouteNode>();
            var query = new SearchParameters
            {
                Text = text,
                QueryBy = "name"
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

        private async Task<List<Address>> SearchAddress(string text)
        {
            List<Address> addresses = new List<Address>();
            var adressQuery = new SearchParameters
            {
                Text = text,
                QueryBy = "accessAddressDescription"
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