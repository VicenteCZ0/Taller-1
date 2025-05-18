using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.Dtos;
using APITaller1.src.data;
using APITaller1.src.interfaces;
using APITaller1.src.Mappers;
using APITaller1.src.models;

using Microsoft.EntityFrameworkCore;

namespace APITaller1.src.Repositories
{
    public class UserRepository(StoreContext store) : IUserRepository
    {
        private readonly StoreContext _context = store;

        public async Task CreateUserAsync(User user, ShippingAddress? shippingAddress)
        {
            if (shippingAddress != null)
            {
                user.ShippingAddress = shippingAddress;
                shippingAddress.User = user;
                await _context.ShippingAddress.AddAsync(shippingAddress);
            }

            await _context.Users.AddAsync(user);
        }

        public void DeleteUserAsync(User user)
        {
            if (user.ShippingAddress != null)
            {
                _context.ShippingAddress.Remove(user.ShippingAddress);
            }
            _context.Users.Remove(user);
        }
        
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Include(x => x.ShippingAddress)
                .ToListAsync();

            return users.Select(UserMapper.MapToDTO);
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users
                .Include(x => x.ShippingAddress)
                .FirstOrDefaultAsync(x => x.Id == userId);

            return user != null ? UserMapper.MapToDTO(user) : null;
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users
                .Include(x => x.ShippingAddress)
                .FirstOrDefaultAsync(x => x.UserName == username);

            return user != null ? UserMapper.MapToDTO(user) : null;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                .Include(x => x.ShippingAddress)
                .FirstOrDefaultAsync(x => x.Email == email);

            return user != null ? UserMapper.MapToDTO(user) : null;
        }

        public void UpdateShippingAddressAsync(int userId, ShippingAddressDto shippingAddressDto)
        {
            var user = _context.Users
                .Include(x => x.ShippingAddress)
                .FirstOrDefault(x => x.Id == userId)
                ?? throw new Exception("User not found");

            if (user.ShippingAddress == null)
            {
                user.ShippingAddress = new ShippingAddress
                {
                    Street = shippingAddressDto.Street,
                    Number = shippingAddressDto.Number,
                    Commune = shippingAddressDto.Commune,
                    Region = shippingAddressDto.Region,
                    PostalCode = shippingAddressDto.PostalCode,
                    User = user
                };

                _context.ShippingAddress.Add(user.ShippingAddress);
            }
            else
            {
                user.ShippingAddress.Street = shippingAddressDto.Street;
                user.ShippingAddress.Number = shippingAddressDto.Number;
                user.ShippingAddress.Commune = shippingAddressDto.Commune;
                user.ShippingAddress.Region = shippingAddressDto.Region;
                user.ShippingAddress.PostalCode = shippingAddressDto.PostalCode;

                _context.ShippingAddress.Update(user.ShippingAddress);
            }

            _context.Users.Update(user);
        }

        public void UpdateUserAsync(User user)
        {
            var existingUser = _context.Users
                .FirstOrDefault(x => x.Id == user.Id)
                ?? throw new Exception("User not found");

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Telephone = user.Telephone;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.AccountStatus = user.AccountStatus;
            existingUser.DeactivationReason = user.DeactivationReason;
            existingUser.LastLogin = user.LastLogin;

            _context.Users.Update(existingUser);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}