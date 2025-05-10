using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITaller1.src.models
{
    public class User
    {
        public int UserID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string Telephone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool AccountStatus { get; set; } 
        public DateTime LastLogin { get; set; }
        public int RoleID { get; set; }
        // Navigation properties
        public ICollection<ShippingAddress> ShippingAddress { get; set; } = new List<ShippingAddress>();

        public Role Role { get; set; }
        
    }
}