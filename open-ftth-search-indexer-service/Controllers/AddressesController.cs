using System;
using Microsoft.AspNetCore.Mvc;
using open_ftth_search_indexer_service.Models;
using Typesense;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

namespace openftth_search_indexer_service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private ITypesenseClient _client;

        public AddressesController(ITypesenseClient client)
        {
            _client = client;
        }
        [HttpGet("{field}/{accessAddressDescription}")]
        public async Task<ActionResult<List<Address>>> Get(string accessAddressDescription,string field)
        {
            List<Address> addresses = new List<Address>();
            var query = new SearchParameters
            {
                Text = accessAddressDescription,
                QueryBy = field
            };

            var result = await _client.Search<Address>("Addresses", query);
            foreach (var res in result.Hits)
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