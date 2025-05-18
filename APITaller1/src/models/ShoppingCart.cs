using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITaller1.src.models
{
    public class ShoppingCart
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        // Relaciones
        public User User { get; set; }
        public ICollection<CartItem>? CartItems { get; set; } = new List<CartItem>();
    }
}
