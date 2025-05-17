using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APITaller1.src.models;

namespace APITaller1.src.interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int roleId);
        Task<Role> GetRoleByNameAsync(string roleName);
        Task CreateRoleAsync(Role role);
        Task UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(int roleId);
    }
}