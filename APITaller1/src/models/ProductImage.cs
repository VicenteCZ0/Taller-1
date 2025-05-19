using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITaller1.src.models;

public class ProductImage
{
    public int ImageID { get; set; }  
    public string Url_Image { get; set; } = string.Empty;
    
    public int ProductID { get; set; } 
    
    // Propiedad de navegación
    public Product Product { get; set; } = null!;
}