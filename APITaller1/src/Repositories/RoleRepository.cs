using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.data;
using APITaller1.src.interfaces;
using APITaller1.src.models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APITaller1.src.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly StoreContext _context;
        
        private readonly ILogger<Product> _logger;

        public RoleRepository(StoreContext store, ILogger<Product> logger)
    {
        _context = store;
        _logger = logger;
    }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RolName == roleName);
        }

        public async Task CreateRoleAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
        }

        public Task UpdateRoleAsync(Role role)
        {
            _context.Roles.Update(role);
            return Task.CompletedTask;
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role != null)
            {
                _context.Roles.Remove(role);
            }
        }
    }
}