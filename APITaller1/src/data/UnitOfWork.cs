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

// Task SaveChangeAsync() → Task<int> SaveChangeAsync() para saber cuantos cambios se hicieron 
// se cambio a un construcctor clasico para que se realice una una asignacion a cada parametro recibido a las propiedades internas de la clase 
        public UnitOfWork(
            StoreContext context,
            IUserRepository userRepository,
            IProductRepository productRepository,
            IShoppingCartRepository shoppingCartRepository
        )
        {
            _context = context;
            UserRepository = userRepository;
            ProductRepository = productRepository;
            ShoppingCartRepository = shoppingCartRepository;
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
