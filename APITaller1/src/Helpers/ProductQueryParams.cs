namespace APITaller1.src.Helpers
{
    
    public class ProductQueryParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public string? Status { get; set; } // "Nuevo" o "Usado"
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Search { get; set; } // Título, descripción, características
        public string? Sort { get; set; } // "priceAsc", "priceDesc", "nameAsc", "nameDesc"
    }


}
