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

namespace APITaller1.src.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly StoreContext _context;

    public ShoppingCartRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<ShoppingCart?> GetByUserIdAsync(int userId)
    {
        return await _context.ShoppingCarts
        .Include(sc => sc.CartItems)
        .ThenInclude(ci => ci.Product)
        .FirstOrDefaultAsync(sc => sc.UserId == userId);
    }


    public async Task<ShoppingCart> GetOrCreateCartAsync(int userId)
    {
        var cart = await GetByUserIdAsync(userId);

        if (cart == null)
        {

            cart = new ShoppingCart
            {
                UserId = userId,
                CartItems = new List<CartItem>()
            };

            await _context.ShoppingCarts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        return cart;
    }

    public async Task AddAsync(ShoppingCart cart)
    {
        await _context.ShoppingCarts.AddAsync(cart);
    }
    

    public async Task<ShoppingCart> GetByUserIdWithItemsAndProductsAsync(int userId)
    {
        return await _context.ShoppingCarts
            .Include(sc => sc.CartItems)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(sc => sc.UserId == userId);
    }

}
