using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace APITaller1.src.models
{
    public class OrderItem
    {
        public int ID { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        [ForeignKey("Order")] 
        public int OrderID { get; set; }
        public Order Order { get; set; } = null!;

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public Product Product { get; set; } = null!;
    }
}
