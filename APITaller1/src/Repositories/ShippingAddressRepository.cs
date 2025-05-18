using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APITaller1.src.models;
using APITaller1.src.interfaces;
using APITaller1.src.data;

namespace APITaller1.src.Repositories
{
    public class ShippingAddressRepository : IShippingAddressRepository
    {
        private readonly StoreContext _context;
        private readonly ILogger<Product> _logger;

        public ShippingAddressRepository(StoreContext store, ILogger<Product> logger)
        {
            _context = store;
            _logger = logger;
        }

        public async Task<IEnumerable<ShippingAddress>> GetAllShippingAddressesAsync()
        {
            return await _context.ShippingAddress
                .Include(a => a.User)
                .ToListAsync();
        }

        public async Task<ShippingAddress> GetShippingAddressByIdAsync(int addressId)
        {
            return await _context.ShippingAddress
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.AddressID == addressId);
        }

        public async Task<IEnumerable<ShippingAddress>> GetShippingAddressesByUserIdAsync(int userId)
        {
            return await _context.ShippingAddress
                .Where(a => a.UserId == userId)
                .Include(a => a.User)
                .ToListAsync();
        }

        public async Task<ShippingAddress> AddShippingAddressAsync(ShippingAddress shippingAddress)
        {
            _context.ShippingAddress.Add(shippingAddress);
            await _context.SaveChangesAsync();
            return shippingAddress;
        }

        public async Task<ShippingAddress> UpdateShippingAddressAsync(ShippingAddress shippingAddress)
        {
            _context.Entry(shippingAddress).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return shippingAddress;
        }

        public async Task<bool> DeleteShippingAddressAsync(int addressId)
        {
            var shippingAddress = await _context.ShippingAddress.FindAsync(addressId);
            
            if (shippingAddress == null)
                return false;
                
            _context.ShippingAddress.Remove(shippingAddress);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ShippingAddressExistsAsync(int addressId)
        {
            return await _context.ShippingAddress.AnyAsync(a => a.AddressID == addressId);
        }
    }
}