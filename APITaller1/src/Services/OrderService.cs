using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITaller1.src.Dtos;
using APITaller1.src.data;
using APITaller1.src.models;
using APITaller1.src.Mappers;

namespace APITaller1.src.Services
{
    public class OrderService
    {
        private readonly UnitOfWork _unitOfWork;

        public OrderService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> CreateOrderAsync(int userId)
        {
            var cart = await _unitOfWork.ShoppingCartRepository.GetByUserIdAsync(userId);
            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                throw new InvalidOperationException("No hay productos en el carrito.");
            }

            var orderItems = cart.CartItems.Select(item => new OrderItem
            {
                ProductID = item.ProductID,
                Quantity = item.Quantity,
                UnitPrice = item.Product?.Price ?? 0
            }).ToList();

            var total = orderItems.Sum(oi => oi.Quantity * oi.UnitPrice);

            var order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = "Pendiente",
                TotalAmount = total,
                OrderItems = orderItems
            };

            
            await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.SaveChangeAsync();

            foreach (var orderItem in orderItems)
            {
                orderItem.OrderID = order.ID;
                await _unitOfWork.OrderItemRepository.AddAsync(orderItem);
            }
            
            await _unitOfWork.CartItemRepository.ClearCartAsync(cart.ID);
            await _unitOfWork.SaveChangeAsync();

            return order;
        }
        public async Task<List<OrderDto>> GetOrdersByUserAsync(int userId)
        {
            var orders = await _unitOfWork.OrderRepository.GetByUserAsync(userId);

            return orders.Select(order => new OrderDto
            {
                OrderId = order.ID,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductID,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            }).ToList();
        }

    }
}
