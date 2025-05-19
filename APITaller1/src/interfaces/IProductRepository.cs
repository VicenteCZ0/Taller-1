using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APITaller1.src.models;
using APITaller1.src.Helpers;

namespace APITaller1.src.interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task UpdateProductAsync(Product product);

        Task<bool> HasOrdersAsync(int productId);

        // Métodos adicionales opcionales:
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
        Task<IEnumerable<Product>> SearchProductsByNameAsync(string keyword);


        Task<PagedList<Product>> GetCatalogAsync(ProductQueryParams queryParams);
        Task<PagedList<Product>> GetAdminListAsync(AdminProductQueryParams queryParams);
        Task<Product?> GetByIdWithImagesAsync(int id);
        Task<Product?> GetByIdAsync(int id);

        Task<bool> Exists(int productId);

        void Remove(Product product);

    }
}
