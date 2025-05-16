using APITaller1.src.Dtos;
using APITaller1.src.models;

namespace APITaller1.src.Mappers
{
    public static class ProductImageMapper
    {
        public static ProductImageDto ToDto(ProductImage image)
        {
            return new ProductImageDto
            {
                ImageID = image.ImageID,
                Url_Image = image.Url_Image,
                ProductID = image.ProductID
            };
        }

        public static void UpdateModel(ProductImage image, ProductImageDto dto)
        {
            image.Url_Image = dto.Url_Image;
            image.ProductID = dto.ProductID;
        }
    }
}
