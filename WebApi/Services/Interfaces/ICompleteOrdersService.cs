using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface ICompleteOrdersService
    {
        IEnumerable<CompletationOrder> GetOrders();
        Task<CompletationOrder> GetOrderAsync(int id);
        Task<CompletationOrder> AddOrderAsync(CompletationOrder order);
        Task<CompletationOrder> UpdateOrderAsync(CompletationOrder order, int id);
        Task<CompletationOrder> DeleteOrderAsync(int id);
    }
}
