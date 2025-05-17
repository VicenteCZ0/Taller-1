
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.Dtos;
using APITaller1.src.models;

namespace APITaller1.src.Mappers
{
    public static class OrderMapper
    {
        public static Order ToModel(OrderCreationDto dto, string userId, List<Product> productos)
        {
            var items = dto.Items.Select(item =>
            {
                var producto = productos.First(p => p.ProductID == item.ProductID);
                return new OrderItem
                {
                    ProductID = producto.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = producto.Price
                };
            }).ToList();

            var total = items.Sum(i => i.Quantity * i.UnitPrice);

            return new Order
            {
                UserId = userId,
                Status = "Pendiente",
                TotalAmount = total,
                OrderItems = items
            };
        }

        public static OrderDetailsDto ToDto(Order order)
        {
            return new OrderDetailsDto
            {
                ID = order.ID,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(i => new OrderItemShowDto
                {
                    ProductID = i.ProductID,
                    ProductName = i.Product?.Name ?? "Sin nombre",
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };
        }
    }
}
