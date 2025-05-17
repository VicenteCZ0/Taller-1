using APITaller1.src.Dtos;
using APITaller1.src.models;

namespace APITaller1.src.Mappers
{
    public class UserMapper
    {
        public static UserDto MapToDTO(User user)
        {
            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Telephone = user.Telephone,
                DateOfBirth = user.DateOfBirth,
                AccountStatus = user.AccountStatus,
                LastLogin = user.LastLogin,

                ShippingAddress = user.ShippingAddress != null
                    ? new ShippingAddressDto
                    {
                        AddressID = user.ShippingAddress.AddressID,
                        Street = user.ShippingAddress.Street,
                        Number = user.ShippingAddress.Number,
                        Commune = user.ShippingAddress.Commune,
                        Region = user.ShippingAddress.Region,
                        PostalCode = user.ShippingAddress.PostalCode,
                    }
                    : null
            };
        }
    }
}
