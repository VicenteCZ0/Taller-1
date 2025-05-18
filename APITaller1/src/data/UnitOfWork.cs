using System.Threading.Tasks;
using APITaller1.src.interfaces;

namespace APITaller1.src.data
{
    public class UnitOfWork
    {
        private readonly StoreContext _context;

        public IUserRepository UserRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IProductImageRepository ProductImageRepository { get; }
        public IStatusRepository StatusRepository { get; }
        public IShoppingCartRepository ShoppingCartRepository { get; }
        public ICartItemRepository CartItemRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IOrderItemRepository OrderItemRepository { get; }


        public UnitOfWork(
            StoreContext context,
            IUserRepository userRepository,
            IProductRepository productRepository,
            IProductImageRepository productImageRepository,
            IStatusRepository statusRepository,
            IShoppingCartRepository shoppingCartRepository,
            ICartItemRepository cartItemRepository,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository

        )
        {
            _context = context;
            UserRepository = userRepository;
            ProductRepository = productRepository;
            ProductImageRepository = productImageRepository;
            StatusRepository = statusRepository;
            ShoppingCartRepository = shoppingCartRepository;
            CartItemRepository = cartItemRepository;
            OrderRepository = orderRepository;
            OrderItemRepository = orderItemRepository;
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
