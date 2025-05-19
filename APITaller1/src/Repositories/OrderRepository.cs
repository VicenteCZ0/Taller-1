using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APITaller1.src.data;
using APITaller1.src.interfaces;
using APITaller1.src.models;

namespace APITaller1.src.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly StoreContext _context;

        public OrderRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetByUserAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.ID == orderId);
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }


        public async Task<List<Order>> GetByUserWithFiltersAsync(int userId, DateTime? fromDate, DateTime? toDate, decimal? minTotal, decimal? maxTotal)
        {
            var query = _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(o => o.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(o => o.CreatedAt <= toDate.Value);

            if (minTotal.HasValue)
                query = query.Where(o => o.TotalAmount >= minTotal.Value);

            if (maxTotal.HasValue)
                query = query.Where(o => o.TotalAmount <= maxTotal.Value);

            return await query.ToListAsync();
        }

    }
}
