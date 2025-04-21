using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.models;

namespace APITaller1.src.interfaces;

public interface IProductRepository
{
    Task<Product> GetProductByIdAsync(int id);
    Task<IEnumerable<Product>> GetProductsAsync();
    Task AddProductAsync(Product product);
    void DeleteProductAsync(Product product);
    Task UpdateProductAsync(Product product);
}