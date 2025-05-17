using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace APITaller1.src.Dtos
{
    public class OrderItemDto
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
