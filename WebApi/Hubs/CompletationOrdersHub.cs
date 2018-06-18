using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Hubs.Interfaces;
using WebApi.Models;

namespace WebApi.Hubs
{
    public class CompletationOrdersHub : Hub<ICompletationOrdersClient>
    {
        public async Task Add(CompletationOrder value) => await Clients.All.Add(value);

        public async Task Update(CompletationOrder value) => await Clients.All.Update(value);

        public async Task Delete(CompletationOrder value) => await Clients.All.Delete(value);
    }
}
