using System;

namespace APITaller1.src.Dtos
{
    public class OrderFilterDto
    {
        public DateTime? FromDate { get; set; } 
        public DateTime? ToDate { get; set; }   
        public decimal? MinTotal { get; set; }    
        public decimal? MaxTotal { get; set; }   
    }
}
