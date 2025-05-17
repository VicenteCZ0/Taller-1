using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITaller1.src.models
{
    public class CartItem
    {
        public int ID { get; set; }

        public int Quantity { get; set; }

        public int ShoppingCartID { get; set; }
        public int ProductID { get; set; }

        // Relaciones
        public ShoppingCart ShoppingCart { get; set; }
        public Product Product { get; set; }
    }
}
