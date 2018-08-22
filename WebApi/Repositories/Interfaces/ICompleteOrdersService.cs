using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models.Completation;

namespace WebApi.Repositories.Interfaces
{
    public interface IServeCompletationOrders
    {
        IEnumerable<CompletationOrder> GetOrders();
        Task<CompletationOrder> GetOrderAsync(int id);
        Task<CompletationOrder> AddOrderAsync(CompletationOrder order);
        Task<CompletationOrder> UpdateOrderAsync(CompletationOrder order, int id);
        Task<CompletationOrder> DeleteOrderAsync(int id);
    }
}
