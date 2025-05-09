using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITaller1.src.models
{
    public class ShippingAddress
    {
        public int AddressID { get; set; }
        public required string Street { get; set; }
        public required string Number { get; set; }
        public required string Commune {get; set;}
        public required string Region { get; set; }
        public required string PostalCode { get; set; }

        // Navigation properties
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}