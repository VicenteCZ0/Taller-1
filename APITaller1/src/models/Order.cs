using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITaller1.src.models;
public class Order
{
    public int ID { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pendiente"; // Pendiente, Enviado, Cancelado, etc.
    public decimal TotalAmount { get; set; }

    // Foreign Key
    public string UserId { get; set; }
    public User User { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
}