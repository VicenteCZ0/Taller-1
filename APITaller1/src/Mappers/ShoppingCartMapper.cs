using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.Dtos;
using APITaller1.src.models;

namespace APITaller1.src.Mappers
{
    public static class ShoppingCartMapper
    {
        public static BasketDto ToDto(this ShoppingCart cart)
        {
            return new BasketDto
            {
                ShoppingCartId = cart.ID,
                UserId = cart.UserID,
                Items = cart.CartItems?.Select(item => new CartItemDto
                {
                    ProductId = item.ProductID,
                    Quantity = item.Quantity,
                    ProductName = item.Product?.Name,
                    UnitPrice = item.Product?.Price
                }).ToList() ?? new()
            };
        }
    }
}
