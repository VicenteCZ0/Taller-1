using APITaller1.src.Dtos;
using APITaller1.src.models;

namespace APITaller1.src.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto MapToDTO(Product product)
        {
            return new ProductDto
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                CreatedAt = product.CreatedAt,
                Urls = product.Urls,
                Brand = product.Brand,
                StatusName = product.Status.StatusName, 
                ImageUrls = product.ProductImages.Select(img => img.Url_Image).ToList()
            };
        }
    }
}
