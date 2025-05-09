using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.data;
using APITaller1.src.Dtos;
using APITaller1.src.models;

using Microsoft.AspNetCore.Mvc;

namespace APITaller1.src.Controllers
{
    public class UserController(ILogger<UserController> logger, UnitOfWork unitOfWork) : BaseController
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly UnitOfWork _context = unitOfWork;

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.UserRepository.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser( CreateUserDto userDto)
        {
            if (userDto.ConfirmPassword != userDto.Password)
            {
                return BadRequest("Passwords do not match");
            }
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = userDto.Password,
                Telephone = userDto.Telephone,
                ShippingAddress = new List<ShippingAddress> {
                    new ShippingAddress
                    {
                        Street = userDto.Street ?? string.Empty,
                        Number = userDto.Number ?? string.Empty,
                        Commune = userDto.Commune ?? string.Empty,
                        Region = userDto.Region ?? string.Empty,
                        PostalCode = userDto.PostalCode ?? string.Empty
                    }
                }
            };
            await _context.UserRepository.CreateUserAsync(user, user.ShippingAddress.FirstOrDefault());
            await _context.SaveChangeAsync();
            return Ok(user);
        }
    }
}