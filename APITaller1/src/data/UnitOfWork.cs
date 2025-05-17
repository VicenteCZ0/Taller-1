using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.interfaces;

namespace APITaller1.src.data;

public class UnitOfWork
{
    private readonly StoreContext _context;

    public IUserRepository UserRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IOrderRepository OrderRepository { get; }
    public ICartItemRepository CartItemRepository { get; }
    public IShoppingCartRepository ShoppingCartRepository { get; }

    public UnitOfWork(
        StoreContext context,
        IUserRepository userRepository,
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        ICartItemRepository cartItemRepository,
        IShoppingCartRepository shoppingCartRepository)
    {
        _context = context;
        UserRepository = userRepository;
        ProductRepository = productRepository;
        OrderRepository = orderRepository;
        CartItemRepository = cartItemRepository;
        ShoppingCartRepository = shoppingCartRepository;
    }

    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }
}
