using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
public class CartItemCreationDto
{
    [Required]
    public int ProductID { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
