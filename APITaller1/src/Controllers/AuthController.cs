using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using APITaller1.src.data;
using APITaller1.src.models;
using APITaller1.src.Services;
using APITaller1.src.Dtos;
using APITaller1.src.Helpers; 

namespace APITaller1.src.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenService _tokenService;
        private readonly ILogger<AuthController> _logger;
        private readonly UnitOfWork _context;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            TokenService tokenService,
            ILogger<AuthController> logger,
            UnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
            _context = unitOfWork;
        }

        private async Task<User?> GetCurrentUserAsync()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return null;

            return await _userManager.FindByIdAsync(userIdClaim);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
                return Unauthorized(new ApiResponse<string>(false, "Correo o contraseña inválidos"));

            // Validar si la cuenta está deshabilitada
            if (!user.AccountStatus)
                return Unauthorized(new ApiResponse<string>(false, "Cuenta deshabilitada. Contacte al administrador."));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new ApiResponse<string>(false, "Correo o contraseña inválidos"));

            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var token = await _tokenService.CreateToken(user);

            return Ok(new ApiResponse<object>(
                true,
                "Inicio de sesión exitoso",
                new { token }
            ));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ApiResponse<string>(
                    false,
                    "Datos inválidos",
                    null,
                    errors
                ));
            }

            var userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
                return BadRequest(new ApiResponse<string>(false, "El correo electrónico ya está en uso."));

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                Telephone = dto.Telephone,
                DateOfBirth = dto.DateOfBirth,
                AccountStatus = true,
                LastLogin = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new ApiResponse<string>(
                    false,
                    "Error al crear el usuario",
                    null,
                    errors
                ));
            }

            await _userManager.AddToRoleAsync(user, "User");

            var userData = new
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return Ok(new ApiResponse<object>(
                true,
                "Usuario registrado exitosamente",
                userData
            ));
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new ApiResponse<string>(true, "Sesión cerrada exitosamente."));
        }
    }
}