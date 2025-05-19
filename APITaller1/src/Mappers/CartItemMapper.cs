using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITaller1.src.models;
using APITaller1.src.Dtos;

namespace APITaller1.src.Mappers
{
    public static class CartItemMapper
    {
        public static CartItemShowDto ToShowDto(CartItem item)
        {
            return new CartItemShowDto
            {
                ProductID = item.ProductID,
                ProductName = item.Product?.Name ?? "Producto desconocido",
                UnitPrice = item.Product?.Price ?? 0,
                Quantity = item.Quantity,
                ProductImageUrl = item.Product?.ProductImages.FirstOrDefault()?.Url_Image
            };
        }
    }
}
