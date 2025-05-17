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
                    user.ShippingAddress = user.ShippingAddress = shippingAddress;
                    shippingAddress.User = user; // importante para mantener la relación
                    await _context.ShippingAddress.AddAsync(shippingAddress);
                }

                await _context.Users.AddAsync(user);
            }

            public void DeleteUserAsync(User user, ShippingAddress shippingAddress)
            {
                _context.ShippingAddress.Remove(shippingAddress);
                _context.Users.Remove(user);
            }
            
            public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
            {
                var users = await _context.Users
                    .Include(x => x.ShippingAddress)
                    .ToListAsync();

                return users.Select(UserMapper.MapToDTO);
            }

            public Task<UserDto> GetUserByIdAsync(string firstName)
            {
                var user = _context.Users
                    .Include(x => x.ShippingAddress)
                    .FirstOrDefault(x => x.FirstName == firstName)
                    ?? throw new Exception("User not found");

                return Task.FromResult(UserMapper.MapToDTO(user));
            }

            public void UpdateShippingAddressAsync(UserDto userDto)
            {
                var user = _context.Users.Include(x => x.ShippingAddress).FirstOrDefault(x => x.FirstName == userDto.FirstName)
                        ?? throw new Exception("User not found");

                var direccionDto = userDto.ShippingAddress;
                if (direccionDto == null)
                {
                    throw new Exception("Shipping address is required");
                }

                if (user.ShippingAddress == null)
                {
                    user.ShippingAddress = new ShippingAddress
                    {
                        Street = direccionDto.Street,
                        Number = direccionDto.Number,
                        Commune = direccionDto.Commune,
                        Region = direccionDto.Region,
                        PostalCode = direccionDto.PostalCode,
                        User = user
                    };

                    _context.ShippingAddress.Add(user.ShippingAddress);
                }
                else
                {
                    user.ShippingAddress.Street = direccionDto.Street;
                    user.ShippingAddress.Number = direccionDto.Number;
                    user.ShippingAddress.Commune = direccionDto.Commune;
                    user.ShippingAddress.Region = direccionDto.Region;
                    user.ShippingAddress.PostalCode = direccionDto.PostalCode;

                    _context.ShippingAddress.Update(user.ShippingAddress);
                }

                _context.Users.Update(user);
            }
                    

            public void UpdateUserAsync(User user)
            {
                var existingUser = _context.Users.FirstOrDefault(x => x.FirstName == user.FirstName) ?? throw new Exception("User not found");
                if (existingUser != null)
                {
                    existingUser.LastName = user.LastName;
                    existingUser.Telephone = user.Telephone;
                    existingUser.Email = user.Email;
                    _context.Users.Update(existingUser);
                }
                else
                {
                    throw new Exception("User not found");
                }
            }
        }
    }