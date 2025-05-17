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
                    user.ShippingAddress = new List<ShippingAddress> { shippingAddress };
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
                    .Include(x => x.Role)  // <-- Asegúrate que se incluya el Role
                    .ToListAsync();

                return users.Select(UserMapper.MapToDTO);
            }

            public Task<UserDto> GetUserByIdAsync(string firstName)
            {
                var user = _context.Users
                    .Include(x => x.ShippingAddress)
                    .Include(x => x.Role)   // Incluye Role aquí también
                    .FirstOrDefault(x => x.FirstName == firstName)
                    ?? throw new Exception("User not found");

                return Task.FromResult(UserMapper.MapToDTO(user));
            }

            public void UpdateShippingAddressAsync(UserDto userDto)
            {
                var user = _context.Users.Include(x => x.ShippingAddress).FirstOrDefault(x => x.FirstName == userDto.FirstName)
                        ?? throw new Exception("User not found");

                var direccion = user.ShippingAddress.FirstOrDefault();

                if (direccion == null)
                {
                    direccion = new ShippingAddress
                    {
                        Street = userDto.ShippingAddresses?.FirstOrDefault()?.Street ?? string.Empty,
                        Number = userDto.ShippingAddresses?.FirstOrDefault()?.Number ?? string.Empty,
                        Commune = userDto.ShippingAddresses?.FirstOrDefault()?.Commune ?? string.Empty,
                        Region = userDto.ShippingAddresses?.FirstOrDefault()?.Region ?? string.Empty,
                        PostalCode = userDto.ShippingAddresses?.FirstOrDefault()?.PostalCode ?? string.Empty,
                        User = user // Asignar la relación
                    };

                    user.ShippingAddress = new List<ShippingAddress> { direccion };
                    _context.ShippingAddress.Add(direccion);
                }
                else
                {
                    direccion.Street = userDto.ShippingAddresses?.FirstOrDefault()?.Street ?? string.Empty;
                    direccion.Number = userDto.ShippingAddresses?.FirstOrDefault()?.Number ?? string.Empty;
                    direccion.Commune = userDto.ShippingAddresses?.FirstOrDefault()?.Commune ?? string.Empty;
                    direccion.Region = userDto.ShippingAddresses?.FirstOrDefault()?.Region ?? string.Empty;
                    direccion.PostalCode = userDto.ShippingAddresses?.FirstOrDefault()?.PostalCode ?? string.Empty;

                    _context.ShippingAddress.Update(direccion);
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