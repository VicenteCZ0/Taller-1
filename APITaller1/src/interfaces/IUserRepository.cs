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
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user, ShippingAddress? shippingAddress);
        void UpdateUserAsync(User user);
        void UpdateShippingAddressAsync(int userId, ShippingAddressDto shippingAddressDto);
        void DeleteUserAsync(User user);
        Task<bool> SaveChangesAsync();

        Task<User> GetUserWithShippingAddressAsync(int userId);
    }
}