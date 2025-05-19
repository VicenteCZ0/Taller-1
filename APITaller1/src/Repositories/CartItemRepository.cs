using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITaller1.src.data;
using APITaller1.src.interfaces;
using APITaller1.src.models;

using Microsoft.EntityFrameworkCore;

namespace APITaller1.src.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly StoreContext _context;

        public CartItemRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItem>> GetByCartIdAsync(int cartId)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.ShoppingCartID == cartId)
                .ToListAsync();
        }

        public async Task<CartItem?> GetByCartAndProductAsync(int cartId, int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ShoppingCartID == cartId && ci.ProductID == productId);
        }

        public async Task AddAsync(CartItem item)
        {
            await _context.CartItems.AddAsync(item);
        }

        public async Task UpdateAsync(CartItem item)
        {
            _context.CartItems.Update(item);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(CartItem item)
        {
            _context.CartItems.Remove(item);
            await Task.CompletedTask;
        }

        public async Task ClearCartAsync(int cartId)
        {
            var items = await _context.CartItems
                .Where(ci => ci.ShoppingCartID == cartId)
                .ToListAsync();
                
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public void RemoveRange(IEnumerable<CartItem> cartItems)
        {
            _context.CartItems.RemoveRange(cartItems);
        }
    }
}