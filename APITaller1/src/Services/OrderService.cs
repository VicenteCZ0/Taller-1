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
        private readonly ILogger<OrderService> _logger;

        public OrderService(UnitOfWork unitOfWork, ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Order> CreateOrderAsync(int userId)
        {
            // Iniciar transacción
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            
            try
            {
                // 1. Obtener el carrito con items y productos
                var cart = await _unitOfWork.ShoppingCartRepository
                    .GetByUserIdWithItemsAndProductsAsync(userId);
                
                if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                {
                    throw new InvalidOperationException("No hay productos en el carrito.");
                }

                // 2. Verificar que todos los productos existan
                var missingProductIds = cart.CartItems
                    .Where(item => item.Product == null)
                    .Select(item => item.ProductID)
                    .ToList();
                    
                if (missingProductIds.Any())
                {
                    throw new InvalidOperationException(
                        $"Los siguientes productos no existen: {string.Join(", ", missingProductIds)}");
                }

                // 3. Crear la orden y sus items
                var order = new Order
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Pending",
                    TotalAmount = cart.CartItems.Sum(item => item.Quantity * item.Product.Price),
                    OrderItems = cart.CartItems.Select(item => new OrderItem
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price,
                        Product = item.Product // Asignar la navegación
                    }).ToList()
                };

                // 4. Guardar la orden (esto guardará en cascada los OrderItems)
                await _unitOfWork.OrderRepository.AddAsync(order);
                
                // 5. Limpiar el carrito
                _unitOfWork.CartItemRepository.RemoveRange(cart.CartItems);
                
                // 6. Guardar todos los cambios
                await _unitOfWork.SaveChangeAsync();
                
                // 7. Confirmar transacción
                await transaction.CommitAsync();
                
                return order;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error al crear la orden");
                throw;
            }
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
