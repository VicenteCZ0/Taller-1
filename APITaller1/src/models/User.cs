using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace APITaller1.src.models
{
    public class User : IdentityUser<int>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Telephone { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; }
        public bool AccountStatus { get; set; }
        public string? DeactivationReason { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        
        // Navigation property to ShippingAddress
        public ShippingAddress? ShippingAddress { get; set; }
    }  
}