using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using APITaller1.src.models;
using APITaller1.src.data;
using APITaller1.src.Dtos;
using APITaller1.src.Repositories;
using APITaller1.src.interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace APITaller1.src.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly UnitOfWork _context;

        public AdminController(UserManager<User> userManager, UnitOfWork context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET /api/admin/users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.UserRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("users/detail")]
        public async Task<IActionResult> GetUserById([FromBody] IdDto dto) // ← Usa IdDto aquí
        {
            var user = await _userManager.Users
                .Include(u => u.ShippingAddress)
                .FirstOrDefaultAsync(u => u.Id == dto.Id);

            if (user == null) return NotFound("Usuario no encontrado.");

            return Ok(new UserDto {
                UserID = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Telephone = user.Telephone,
                DateOfBirth = user.DateOfBirth,
                AccountStatus = user.AccountStatus,
                LastLogin = user.LastLogin,
                ShippingAddress = user.ShippingAddress != null ? new ShippingAddressDto {
                    AddressID = user.ShippingAddress.AddressID,
                    Street = user.ShippingAddress.Street,
                    Number = user.ShippingAddress.Number,
                    Commune = user.ShippingAddress.Commune,
                    Region = user.ShippingAddress.Region,
                    PostalCode = user.ShippingAddress.PostalCode
                } : null
            });
        }

        // PUT /api/admin/users/{id}/status
        [HttpPut("users/status")]
        public async Task<IActionResult> ChangeUserStatus([FromBody] ChangeUserStatusDto dto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == dto.Id);
            if (user == null) return NotFound("Usuario no encontrado.");

            if (await _userManager.IsInRoleAsync(user, "Admin") && !dto.Enable)
            {
                var adminsActivos = (await _userManager.GetUsersInRoleAsync("Admin"))
                                    .Count(a => a.AccountStatus && a.Id != user.Id); 
                if (adminsActivos == 0) // Si es el único admin activo
                {
                    return BadRequest("No se puede desactivar al único administrador activo.");
                }
            }

            if (!dto.Enable && string.IsNullOrWhiteSpace(dto.Reason))
                return BadRequest("Debe indicar el motivo de desactivación.");

            user.AccountStatus = dto.Enable;
            user.DeactivationReason = !dto.Enable ? dto.Reason ?? "No especificado" : null;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest("Error al actualizar el usuario.");

            return Ok($"Cuenta {(dto.Enable ? "habilitada" : "deshabilitada")}.");
        }

        // DELETE /api/admin/users
        [HttpDelete("users")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserDto dto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == dto.Id);
            if (user == null) return NotFound("Usuario no encontrado.");

            // Validar si es el último admin
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var admins = await _userManager.GetUsersInRoleAsync("Admin");
                if (admins.Count(a => a.AccountStatus) <= 1)
                {
                    return BadRequest("No se puede desactivar al único administrador activo.");
                }
            }

            user.AccountStatus = false;
            user.DeactivationReason = dto.Reason;
            
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest("Error al desactivar el usuario.");

            return Ok(new { 
                message = "Usuario desactivado exitosamente.",
                reason = dto.Reason,
                userId = dto.Id 
            });
        }
        
        [HttpPost("users/filter")]
        public async Task<IActionResult> FilterUsers([FromBody] UserFilterDto dto)
        {
            var query = _userManager.Users.AsQueryable();

            if (dto.AccountStatus.HasValue)
                query = query.Where(u => u.AccountStatus == dto.AccountStatus.Value);

            if (!string.IsNullOrEmpty(dto.Email))
                query = query.Where(u => u.Email.Contains(dto.Email));

            if (!string.IsNullOrEmpty(dto.Name))
                query = query.Where(u => u.FirstName.Contains(dto.Name) || u.LastName.Contains(dto.Name));

            if (dto.RegisteredAfter.HasValue)
                query = query.Where(u => u.RegisteredAt >= dto.RegisteredAfter.Value);

            if (dto.RegisteredBefore.HasValue)
                query = query.Where(u => u.RegisteredAt <= dto.RegisteredBefore.Value);

            var total = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.Id)
                .Skip((dto.Page - 1) * 20)
                .Take(20)
                .ToListAsync();

            return Ok(new
            {
                Total = total,
                Page = dto.Page,
                Results = users.Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.AccountStatus,
                    u.LastLogin,
                    u.RegisteredAt
                })
            });
        }
    }
}