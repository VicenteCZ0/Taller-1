using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.models;
public interface ICartItemRepository
{
    Task<IEnumerable<CartItem>> GetByCartIdAsync(int cartId);
    Task<CartItem?> GetByCartAndProductAsync(int cartId, int productId);
    Task AddAsync(CartItem item);
    Task UpdateAsync(CartItem item);
    Task DeleteAsync(CartItem item);
    void RemoveRange(IEnumerable<CartItem> cartItems);
    Task ClearCartAsync(int cartId);
    Task RemoveAsync(CartItem cartItem);
}