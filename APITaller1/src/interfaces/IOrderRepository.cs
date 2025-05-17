using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.models;

namespace APITaller1.src.interfaces;
public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetByUserAsync(string userId);
    Task<Order?> GetByIdAsync(int orderId);
    Task AddAsync(Order order);
}
