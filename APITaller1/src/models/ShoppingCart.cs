using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace APITaller1.src.models
{
    public class ShoppingCart
    {
        public int ID { get; set; }

        public int UserId { get; set; }

        // Relaciones
        [ForeignKey("UserId")]
        public User User { get; set; }
        public ICollection<CartItem>? CartItems { get; set; } = new List<CartItem>();
    }
}
