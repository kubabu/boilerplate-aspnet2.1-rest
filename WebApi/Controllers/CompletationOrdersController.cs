using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using WebApi.Hubs;
using WebApi.Models.Completation;
using WebApi.Repositories.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CompletationOrdersController: Controller
    {
        readonly ILogger<CompletationOrdersController> _logger;
        private readonly IHubContext<CompletationOrdersHub> _context;
        private readonly IServeCompletationOrders _ordersService;


        public CompletationOrdersController(ILogger<CompletationOrdersController> logger,
            IHubContext<CompletationOrdersHub> hub,
            IServeCompletationOrders ordersService)
        {
            _context = hub;
            _logger = logger;
            _ordersService = ordersService;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<CompletationOrder> Get()
        {
            return _ordersService.GetOrders();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<CompletationOrder> Get(int id)
        {
            var order = await _ordersService.GetOrderAsync(id);
            return order;
        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody]CompletationOrder value)
        {
            if(value != null)
            {
                var order = await _ordersService.AddOrderAsync(value);
                await _context.Clients.All.SendAsync("Add", order);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CompletationOrder value)
        {
            try
            {
                var updated = await _ordersService.UpdateOrderAsync(value, id);
                await _context.Clients.All.SendAsync("Update", updated);
            } 
            catch (Exception)
            {
                // TODO use exception to get more readable return codes
                return NotFound();
            }
            return NoContent();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var item = await _ordersService.DeleteOrderAsync(id);
                await _context.Clients.All.SendAsync("Delete", item);
            }
            catch (Exception)
            {
                // TODO use exception to get more readable return codes
                return NotFound();
            }
            return NoContent();
        }

    }
}