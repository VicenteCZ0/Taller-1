using System;

namespace APITaller1.src.Dtos
{
    public class OrderFilterDto
    {
        public DateTime? FromDate { get; set; }     // Fecha mínima
        public DateTime? ToDate { get; set; }       // Fecha máxima
        public decimal? MinTotal { get; set; }      // Total mínimo del pedido
        public decimal? MaxTotal { get; set; }      // Total máximo del pedido
    }
}
