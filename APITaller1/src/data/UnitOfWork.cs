using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.interfaces;

namespace APITaller1.src.data;

public class UnitOfWork(
    StoreContext context,
    IProductRepository productRepository,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IProductImageRepository productImageRepository,
    IStatusRepository statusRepository)
{
    private readonly StoreContext _context = context;

    public IUserRepository UserRepository { get; set; } = userRepository;
    public IProductRepository ProductRepository { get; set; } = productRepository;
    public IRoleRepository RoleRepository { get; set; } = roleRepository;

    public IProductImageRepository ProductImageRepository { get; set; } = productImageRepository;

    public IStatusRepository StatusRepository { get; set; } = statusRepository;

    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }
}
