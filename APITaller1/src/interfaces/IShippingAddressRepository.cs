using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APITaller1.src.models;

namespace APITaller1.src.interfaces
{
    public interface IShippingAddressRepository
    {
        // Get all shipping addresses
        Task<IEnumerable<ShippingAddress>> GetAllShippingAddressesAsync();
        
        // Get shipping address by ID
        Task<ShippingAddress> GetShippingAddressByIdAsync(int addressId);
        
        // Get shipping addresses by user ID
        Task<IEnumerable<ShippingAddress>> GetShippingAddressesByUserIdAsync(int userId);
        
        // Add a new shipping address
        Task<ShippingAddress> AddShippingAddressAsync(ShippingAddress shippingAddress);
        
        // Update an existing shipping address
        Task<ShippingAddress> UpdateShippingAddressAsync(ShippingAddress shippingAddress);
        
        // Delete a shipping address
        Task<bool> DeleteShippingAddressAsync(int addressId);
        
        // Check if a shipping address exists
        Task<bool> ShippingAddressExistsAsync(int addressId);
    }
}