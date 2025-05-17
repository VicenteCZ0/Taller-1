using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.interfaces;
namespace APITaller1.src.data
{
    public class UnitOfWork
    {
        private readonly StoreContext _context;

        public IUserRepository UserRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IShoppingCartRepository ShoppingCartRepository { get; }
        public ICartItemRepository CartItemRepository { get; } 

        public UnitOfWork(
            StoreContext context,
            IUserRepository userRepository,
            IProductRepository productRepository,
            IShoppingCartRepository shoppingCartRepository,
            ICartItemRepository cartItemRepository 
        )
        {
            _context = context;
            UserRepository = userRepository;
            ProductRepository = productRepository;
            ShoppingCartRepository = shoppingCartRepository;
            CartItemRepository = cartItemRepository; 
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
