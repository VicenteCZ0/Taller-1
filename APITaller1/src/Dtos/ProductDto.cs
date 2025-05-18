using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITaller1.src.Dtos
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string[]? Urls { get; set; }
        public string Brand { get; set; } = null!;
        public string StatusName { get; set; } = null!;

        public List<string> ImageUrls { get; set; } = new();
    }
}
