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

        public async Task<OrderDto> CreateOrderAsync(int userId)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 1. Obtener el carrito con items y productos
                var cart = await _unitOfWork.ShoppingCartRepository
                    .GetByUserIdWithItemsAndProductsAsync(userId);

                if (cart == null)
                {
                    throw new InvalidOperationException($"No se encontró carrito para el usuario {userId}");
                }

                // 2. Verificar productos
                var missingProducts = cart.CartItems
                    .Where(item => item.Product == null)
                    .Select(item => item.ProductID)
                    .ToList();

                if (missingProducts.Any())
                {
                    throw new InvalidOperationException(
                        $"Productos no encontrados: {string.Join(", ", missingProducts)}");
                }

                // 3. Crear la entidad Order
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
                        Product = item.Product
                    }).ToList()
                };

                // 4. Guardar la orden
                await _unitOfWork.OrderRepository.AddAsync(order);

                // 5. Limpiar el carrito
                await _unitOfWork.CartItemRepository.ClearCartAsync(cart.ID);

                // 6. Guardar cambios
                await _unitOfWork.SaveChangeAsync();

                // 7. Confirmar transacción
                await transaction.CommitAsync();

                // 8. Mapear a DTO
                return MapToOrderDto(order);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error al crear la orden");
                throw;
            }
        }

        private OrderDto MapToOrderDto(Order order)
        {
            return new OrderDto
            {
                OrderId = order.ID,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductID,
                    ProductName = oi.Product?.Name ?? "Producto no disponible",
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };
        }


        public async Task<List<OrderDto>> GetOrdersByUserAsync(int userId)
        {
            var orders = await _unitOfWork.OrderRepository.GetByUserAsync(userId);
            return orders.Select(MapToOrderDto).ToList();
        }

        public async Task<List<OrderDto>> GetFilteredOrdersAsync(int userId, OrderFilterDto filter)
        {
            var orders = await _unitOfWork.OrderRepository
                .GetByUserWithFiltersAsync(userId, filter.FromDate, filter.ToDate, filter.MinTotal, filter.MaxTotal);

            return orders.Select(MapToOrderDto).ToList();
        }
        
        public async Task<OrderDto?> GetOrderByIdAsync(int userId, int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);

            if (order == null || order.UserId != userId)
                return null;

            return MapToOrderDto(order);
        }


    }
}
