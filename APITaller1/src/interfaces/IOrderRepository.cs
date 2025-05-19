using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITaller1.src.models;


namespace APITaller1.src.interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetByUserAsync(int userId);
        Task<Order?> GetByIdAsync(int orderId);
        Task AddAsync(Order order);

        Task<List<Order>> GetByUserWithFiltersAsync(int userId, DateTime? fromDate, DateTime? toDate, decimal? minTotal, decimal? maxTotal);

    }
}
