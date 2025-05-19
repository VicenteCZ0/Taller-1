using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using APITaller1.src.data;
using APITaller1.src.Dtos;
using APITaller1.src.models;
using Microsoft.EntityFrameworkCore;

namespace APITaller1.src.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly UnitOfWork _context;
        private readonly UserManager<User> _userManager;

        public UserController(
            ILogger<UserController> logger,
            UnitOfWork unitOfWork,
            UserManager<User> userManager)
        {
            _logger = logger;
            _context = unitOfWork;
            _userManager = userManager;
        }

        private async Task<User?> GetCurrentUserAsync()
        {
            var email = User.Identity?.Name;
            return await _userManager.FindByEmailAsync(email);
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(claim, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("ID de usuario no válido");
        }


        // GET /api/user
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.UserRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("debug")]
        [Authorize]
        public IActionResult DebugClaims()
        {
            return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        [HttpGet("test")]
        [Authorize]
        public IActionResult Test()
        {
            return Ok("Autenticación exitosa.");
        }

        [HttpGet("test-auth")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult TestAuth()
        {
            return Ok("Token válido");
        }

        // GET /api/user/me
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                int userId = GetCurrentUserId();

                var user = await _userManager.Users
                    .Include(u => u.ShippingAddress)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                return user == null 
                    ? NotFound("Usuario no encontrado.") 
                    : Ok(new
                    {
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.Telephone,
                        user.DateOfBirth,
                        Address = user.ShippingAddress != null ? new
                        {
                            user.ShippingAddress.Street,
                            user.ShippingAddress.Number,
                            user.ShippingAddress.Commune,
                            user.ShippingAddress.Region,
                            user.ShippingAddress.PostalCode
                        } : null
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener perfil de usuario");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PUT /api/user/me
        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return NotFound("Usuario no encontrado.");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Telephone = dto.Telephone;
            user.DateOfBirth = dto.DateOfBirth;

            await _userManager.UpdateAsync(user);
            return Ok("Perfil actualizado correctamente.");
        }

        // PUT /api/user/me/password
        [HttpPut("me/password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return NotFound("Usuario no encontrado.");

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return Ok("Contraseña actualizada correctamente.");
        }

        // PUT /api/user/me/address
        [HttpPut("me/address")]
        [Authorize]
        public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressDto dto)
        {
            var userId = GetCurrentUserId();
            var user = await _context.UserRepository.GetUserWithShippingAddressAsync(userId);
            
            if (user == null)
                return NotFound("Usuario no encontrado.");

            if (user.ShippingAddress == null)
            {
                user.ShippingAddress = new ShippingAddress
                {
                    Street = dto.Street,
                    Number = dto.Number,
                    Commune = dto.Commune,
                    Region = dto.Region,
                    PostalCode = dto.PostalCode,
                    UserId = user.Id
                };
            }
            else
            {
                user.ShippingAddress.Street = dto.Street;
                user.ShippingAddress.Number = dto.Number;
                user.ShippingAddress.Commune = dto.Commune;
                user.ShippingAddress.Region = dto.Region;
                user.ShippingAddress.PostalCode = dto.PostalCode;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return Ok("Dirección actualizada correctamente.");
        }
    }
}