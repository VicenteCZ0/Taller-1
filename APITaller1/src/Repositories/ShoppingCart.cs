using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.Dtos;
using APITaller1.src.data;
using APITaller1.src.interfaces;
using APITaller1.src.Mappers;
using APITaller1.src.models;

using Microsoft.EntityFrameworkCore;

namespace APITaller1.src.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly StoreContext _context;

        public ShoppingCartRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<ShoppingCart?> GetCartWithItemsAsync(int userId)
        {
            return await _context.ShoppingCarts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserID == userId);
        }

        public ShoppingCart CreateCart(int userId)
        {
            var cart = new ShoppingCart
            {
                UserID = userId,
                CartItems = new List<CartItem>()
            };

            _context.ShoppingCarts.Add(cart);
            return cart;
        }
    }
}
