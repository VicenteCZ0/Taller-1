using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.data;
using APITaller1.src.interfaces;
using APITaller1.src.models;

using Microsoft.EntityFrameworkCore;

namespace APITaller1.src.Repositories;
public class CartItemRepository : ICartItemRepository
{
    private readonly StoreContext _context;

    public CartItemRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CartItem>> GetItemsByShoppingCartIdAsync(int shoppingCartId)
    {
        return await _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.ShoppingCartID == shoppingCartId)
            .ToListAsync();
    }

    public async Task<CartItem?> GetByIdAsync(int id)
    {
        return await _context.CartItems
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.ID == id);
    }

    public async Task AddAsync(CartItem cartItem)
    {
        await _context.CartItems.AddAsync(cartItem);
    }

    public void Update(CartItem cartItem)
    {
        _context.CartItems.Update(cartItem);
    }

    public void Remove(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
    }
}
