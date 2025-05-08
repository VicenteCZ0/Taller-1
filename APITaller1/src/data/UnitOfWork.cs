using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.interfaces;

namespace APITaller1.src.data;

public class UnitOfWork(StoreContext context, IProductRepository productRepository, IUserRepository userRepository)
{
    private readonly StoreContext _context = context;
    public IUserRepository UserRepository { get; set; } = userRepository;

    public IProductRepository ProductRepository { get; set; } = productRepository;
    
    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }
   
}
