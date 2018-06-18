using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using WebApi.Hubs;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CompletationOrdersController: Controller
    {
        public static List<CompletationOrder> Source { get; set; } = new List<CompletationOrder>();

        readonly ILogger<CompletationOrdersController> _logger;
        private readonly IHubContext<CompletationOrdersHub> _context;


        public CompletationOrdersController(ILogger<CompletationOrdersController> logger, IHubContext<CompletationOrdersHub> hub)
        {
            _context = hub;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<CompletationOrder> Get()
        {
            return Source;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public CompletationOrder Get(int id)
        {
            return Source[id];
        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody]CompletationOrder value)
        {
            if(value != null)
            {
                Source.Add(value);
                await _context.Clients.All.SendAsync("Add", value);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody] CompletationOrder value)
        {
            if (value != null)
            {
                Source[id] = value;
                await _context.Clients.All.SendAsync("Update", value);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            var item = Source[id];
            Source.Remove(item);
            await _context.Clients.All.SendAsync("Delete", item);
        }
    }
}