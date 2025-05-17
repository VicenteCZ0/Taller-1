using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITaller1.src.Dtos
{
    public class UserDto
    {
        public int UserID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Telephone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool AccountStatus { get; set; }
        public DateTime LastLogin { get; set; }
        public ShippingAddressDto? ShippingAddress { get; set; }
    }

}