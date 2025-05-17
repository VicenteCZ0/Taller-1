using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.Dtos;
using APITaller1.src.models;

namespace APITaller1.src.interfaces
{
    public interface IShoppingCartRepository
{
    Task<ShoppingCart?> GetByUserIdAsync(string userId);
    Task<ShoppingCart> GetOrCreateCartAsync(string userId);
    Task AddAsync(ShoppingCart cart);
}

}