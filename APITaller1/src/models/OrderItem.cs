using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITaller1.src.models
{
    public class OrderItem
    {
        public int ID { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public int OrderID { get; set; }
        public Order Order { get; set; } = null!;

        public int ProductID { get; set; }
        public Product Product { get; set; } = null!;
    }
}
