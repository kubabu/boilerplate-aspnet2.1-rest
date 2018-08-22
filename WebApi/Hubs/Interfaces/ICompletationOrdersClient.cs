using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Completation;

namespace WebApi.Hubs.Interfaces
{
    public interface ICompletationOrdersClient
    {
        Task Add(CompletationOrder value);

        Task Update(CompletationOrder value);

        Task Delete(CompletationOrder value);
    }
}
