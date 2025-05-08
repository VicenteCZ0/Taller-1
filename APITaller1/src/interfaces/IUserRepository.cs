using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.Dtos;
using APITaller1.src.models;

namespace APITaller1.src.interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(string firtsName);
        Task CreateUserAsync(User user, ShippingAddres? shippingAddress);
        void UpdateUserAsync(User user);
        void UpdateShippingAddressAsync(UserDto userDto);
        void DeleteUserAsync(User user, ShippingAddres shippingAddress);
    }
}