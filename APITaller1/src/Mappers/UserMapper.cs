using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.Dtos;
using APITaller1.src.models;

namespace APITaller1.src.Mappers
{
public class UserMapper
{
        public static UserDto MapToDTO(User user)
        {
            var firstAddress = user.ShippingAddress?.FirstOrDefault();

            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Telephone = user.Telephone,
                Email = user.Email,

                Street = firstAddress?.Street ?? string.Empty,
                Number = firstAddress?.Number ?? string.Empty,
                Commune = firstAddress?.Commune ?? string.Empty,
                Region = firstAddress?.Region ?? string.Empty,
                PostalCode = firstAddress?.PostalCode ?? string.Empty,
            };
        }
    }

}