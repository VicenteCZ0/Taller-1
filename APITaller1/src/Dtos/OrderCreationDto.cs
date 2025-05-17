using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APITaller1.src.Dtos
{
    public class OrderCreationDto
    {
        public List<OrderItemDto> Items { get; set; }
    }
}