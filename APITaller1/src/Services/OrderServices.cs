using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.Dtos;
using APITaller1.src.data;
using APITaller1.src.interfaces;
using APITaller1.src.Mappers;
using APITaller1.src.models;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly UnitOfWork _unitOfWork;

    public OrderService(
        IOrderRepository orderRepository,
        ICartItemRepository cartItemRepository,
        IProductRepository productRepository,
        IShoppingCartRepository shoppingCartRepository,
        UnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
        _shoppingCartRepository = shoppingCartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderDetailsDto> CreateOrderAsync(string userId)
    {
        var cart = await _shoppingCartRepository.GetByUserIdAsync(userId);
        if (cart == null) throw new Exception("Carrito no encontrado");

        var cartItems = await _cartItemRepository.GetByCartIdAsync(cart.ID);
        if (!cartItems.Any()) throw new Exception("El carrito está vacío");

        var productIds = cartItems.Select(ci => ci.ProductID).ToList();
        var products = (await _productRepository.GetProductsAsync())
            .Where(p => productIds.Contains(p.ProductID)).ToList();

        var order = OrderMapper.ToModel(cartItems, products, userId);

        await _orderRepository.AddAsync(order);
        _cartItemRepository.Clear(cartItems); // implementa este método si no lo tienes
        await _unitOfWork.CompleteAsync();

        return OrderMapper.ToDto(order);
    }

    public async Task<IEnumerable<OrderDetailsDto>> GetOrdersByUserAsync(string userId)
    {
        var orders = await _orderRepository.GetByUserAsync(userId);
        return orders.Select(OrderMapper.ToDto);
    }

    public async Task<OrderDetailsDto?> GetOrderByIdAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return order == null ? null : OrderMapper.ToDto(order);
    }
}
