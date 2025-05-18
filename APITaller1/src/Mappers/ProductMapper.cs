using APITaller1.src.Dtos;
using APITaller1.src.models;

namespace APITaller1.src.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToDto(Product product)
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
                Urls = product.ProductImages.Select(i => i.Url_Image).ToArray(),
                Brand = product.Brand,
                StatusName = product.Status.StatusName,
                ImageUrls = product.ProductImages.Select(i => i.Url_Image).ToList()
            };
        }
        public static Product ToProduct(this ProductCreationDto dto)
        {
            return new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                Category = dto.Category,
                Brand = dto.Brand,
                StatusID = dto.StatusID,
                CreatedAt = DateTime.UtcNow,
            };
        }

        public static ProductDto ToProductDto(this Product product)
        {
            if (product == null) return null;

            return new ProductDto
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                Brand = product.Brand,
                StatusName = product.Status?.StatusName ?? "Desconocido", // Manejo de null
                ImageUrls = product.ProductImages?.Select(i => i.Url_Image).ToList() ?? new List<string>() // Manejo de null
            };
        }


        public static void ApplyTo(this UpdateProductDto dto, Product product)
        {
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.Category = dto.Category;
            product.Brand = dto.Brand;
            product.StatusID = dto.StatusID;
            // Nota: No se asignan las imágenes aquí, eso se hace aparte en el controlador
        }
    }
}
