using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using WebApi.Hubs;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController: Controller
    {
        public static List<string> Source { get; set; } = new List<string>();

        readonly ILogger<ValuesController> _logger;
        private readonly IHubContext<ValuesHub> _context;


        public ValuesController(ILogger<ValuesController> logger, IHubContext<ValuesHub> hub)
        {
            _context = hub;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return Source; //new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return Source[id];
        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody]string value)
        {
            Source.Add(value);
            await _context.Clients.All.SendAsync("Add", value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            Source[id] = value;
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