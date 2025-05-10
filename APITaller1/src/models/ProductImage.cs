using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITaller1.src.models;

public class ProductImage
{
    public int ImageID { get; set; }  // Clave primaria
    public string Url_Image { get; set; } = string.Empty;
    
    // Clave foránea (debe coincidir con Product.ProductID)
    public int ProductID { get; set; }  // Cambiado de ProductId a ProductID
    
    // Propiedad de navegación
    public Product Product { get; set; } = null!;
}