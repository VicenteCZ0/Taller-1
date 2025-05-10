using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITaller1.src.models
{
    public class Product
    {
        public int ProductID { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public required string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public string[]? Urls { get; set; }
        public required string Brand { get; set; }

        // Clave foránea correcta
        public int StatusID { get; set; }

        // Propiedad de navegación
        public Status Status { get; set; } = null!;

        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }

}