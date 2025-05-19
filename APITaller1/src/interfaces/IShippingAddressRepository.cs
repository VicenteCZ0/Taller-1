using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APITaller1.src.models;

namespace APITaller1.src.interfaces
{
    public interface IShippingAddressRepository
    {
        Task<IEnumerable<ShippingAddress>> GetAllShippingAddressesAsync();
        
        Task<ShippingAddress> GetShippingAddressByIdAsync(int addressId);

        Task<IEnumerable<ShippingAddress>> GetShippingAddressesByUserIdAsync(int userId);

        Task<ShippingAddress> AddShippingAddressAsync(ShippingAddress shippingAddress);

        Task<ShippingAddress> UpdateShippingAddressAsync(ShippingAddress shippingAddress);

        Task<bool> DeleteShippingAddressAsync(int addressId);

        Task<bool> ShippingAddressExistsAsync(int addressId);
    }
}