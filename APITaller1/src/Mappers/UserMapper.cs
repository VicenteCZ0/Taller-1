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
                UserID = user.Id, 
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Telephone = user.Telephone,
                RegisteredAt = user.RegisteredAt,
                LastLogin = user.LastLogin,
                AccountStatus = user.AccountStatus,
                DeactivationReason = user.DeactivationReason,
                DateOfBirth = user.DateOfBirth,
                ShippingAddress = user.ShippingAddress != null
                    ? new ShippingAddressDto
                    {
                        Street = user.ShippingAddress.Street,
                        Number = user.ShippingAddress.Number,
                        Commune = user.ShippingAddress.Commune,
                        Region = user.ShippingAddress.Region,
                        PostalCode = user.ShippingAddress.PostalCode
                    }
                    : null
            };
        }
    }
}
