namespace APITaller1.src.Dtos
{
    public class ProductCreationDto
        {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public int StatusID { get; set; }
        public List<IFormFile>? Images { get; set; } // Para subir múltiples imágenes
    }
}
