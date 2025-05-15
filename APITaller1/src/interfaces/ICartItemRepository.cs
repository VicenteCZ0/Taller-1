using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.models;
public interface ICartItemRepository
{
    Task<IEnumerable<CartItem>> GetItemsByShoppingCartIdAsync(int shoppingCartId);
    Task<CartItem?> GetByIdAsync(int id);
    Task AddAsync(CartItem cartItem);
    void Update(CartItem cartItem);
    void Remove(CartItem cartItem);
}
