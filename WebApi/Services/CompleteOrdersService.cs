using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services.Interfaces;


namespace WebApi.Services
{
    public class CompleteOrdersService : ICompleteOrdersService
    {
        static List<CompletationOrder> Source { get; set; } = new List<CompletationOrder>();


        public CompleteOrdersService()
        {

        }


        public async Task<CompletationOrder> GetOrderAsync(int id)
        {
            var order = await find(id);
            return order;
        }

        public IEnumerable<CompletationOrder> GetOrders()
        {
            return Source;
        }

        public async Task<CompletationOrder> AddOrderAsync(CompletationOrder order)
        {
            Source.Add(order);
            var original = await find(order.id);

            return order;
        }

        public async Task<CompletationOrder> UpdateOrderAsync(CompletationOrder order, int id)
        {
            var original = await find(order.id);
            Source.Remove(original);

            return order;
        }

        public async Task<CompletationOrder> DeleteOrderAsync(int id)
        {
            var item = await find(id);
            Source.Remove(item);
            return item;
        }
        

        private Task<CompletationOrder> find(int id)
        {
            return Task<CompletationOrder>.Factory.StartNew(() => Source.Where(s => s.id == id).Single());
        }
    }
}
