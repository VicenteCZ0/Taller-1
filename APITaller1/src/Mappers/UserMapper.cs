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
                UserID = user.UserID,  // <--- Aquí debes asignar UserID desde la entidad

                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Telephone = user.Telephone,
                DateOfBirth = user.DateOfBirth,
                AccountStatus = user.AccountStatus,
                LastLogin = user.LastLogin,
                RoleName = user.Role?.RolName ?? "",

                ShippingAddresses = user.ShippingAddress.Select(addr => new ShippingAddressDto
                {
                    AddressID = addr.AddressID,
                    Street = addr.Street,
                    Number = addr.Number,
                    Commune = addr.Commune,
                    Region = addr.Region,
                    PostalCode = addr.PostalCode,
                    UserId = addr.User?.UserID ?? 0  // <--- También asignar UserId aquí
                }).ToList()
            };
        }

    }
}
