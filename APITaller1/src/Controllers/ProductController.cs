using APITaller1.src.Dtos;
using APITaller1.src.interfaces;
using APITaller1.src.Helpers;
using APITaller1.src.models;
using APITaller1.src.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APITaller1.src.data;


namespace APITaller1.src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IPhotoService _photoService;

    public ProductController(UnitOfWork unitOfWork, IPhotoService photoService)
    {
        _unitOfWork = unitOfWork;
        _photoService = photoService;
    }

    // 1. Exploración del catálogo con paginación y filtros públicos
    [HttpGet("catalog")]
    public async Task<IActionResult> GetCatalog([FromQuery] ProductQueryParams queryParams)
    {
        var products = await _unitOfWork.ProductRepository.GetCatalogAsync(queryParams);
        var productDtos = products.Select(p => p.ToProductDto()).ToList();
        return Ok(productDtos);
    }

    // 2. Visualización de detalles
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdWithImagesAsync(id);
        if (product == null) return NotFound();
        
        return Ok(product.ToProductDto());
    }

    // 3. Listado para administración con paginación
    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public async Task<IActionResult> GetAllAdmin([FromQuery] AdminProductQueryParams queryParams)
    {
        var products = await _unitOfWork.ProductRepository.GetAdminListAsync(queryParams);
        var productDtos = products.Select(p => p.ToProductDto()).ToList();
        return Ok(productDtos);
    }

    // 4. Crear producto
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromForm] ProductCreationDto dto)
    {
        if (dto.Images == null || !dto.Images.Any())
            return BadRequest("Debe subir al menos una imagen.");

        var product = dto.ToProduct();
        await _unitOfWork.ProductRepository.AddProductAsync(product);


        foreach (var image in dto.Images)
        {
            var result = await _photoService.UploadImageAsync(image);
            product.ProductImages.Add(new ProductImage { Url_Image = result, ProductID = product.ProductID });
        }

        await _unitOfWork.SaveChangeAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = product.ProductID }, product.ToProductDto());
    }

    // 5. Actualizar producto
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductDto dto)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdWithImagesAsync(id);
        if (product == null) return NotFound();

        dto.ApplyTo(product);

        if (dto.NewImages != null)
        {
            foreach (var image in dto.NewImages)
            {
                var result = await _photoService.UploadImageAsync(image);
                product.ProductImages.Add(new ProductImage { Url_Image = result, ProductID = id });
            }
        }

        if (dto.RemoveImageIds != null)
        {
            var imagesToRemove = product.ProductImages.Where(pi => dto.RemoveImageIds.Contains(pi.ImageID)).ToList();
            foreach (var img in imagesToRemove)
            {
                await _photoService.DeleteImageAsync(img.Url_Image);
                product.ProductImages.Remove(img);
            }
        }

        await _unitOfWork.SaveChangeAsync();
        
        return Ok(new { message = "Producto actualizado exitosamente", productId = id });  
    }

    // 6. Eliminar producto (soft delete)
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product == null) return NotFound();

/*
        bool hasOrders = await _unitOfWork.ProductRepository.HasOrdersAsync(id);
        if (hasOrders)
        {
            product.StatusID = 3; // Estado "oculto" o similar
        }
        else
        {
            _unitOfWork.ProductRepository.Remove(product);
        }
*/
        await _unitOfWork.SaveChangeAsync();
        return NoContent();
    }
}
