using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APITaller1.src.Services;
using APITaller1.src.Dtos;
using System.Security.Claims;

namespace APITaller1.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CartItemController : ControllerBase
    {
        private readonly CartItemService _cartItemService;

        public CartItemController(CartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var userId = GetUserIdFromClaims();
            var items = await _cartItemService.GetCartItemsAsync(userId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] CartItemCreationDto dto)
        {
            var userId = GetUserIdFromClaims();
            await _cartItemService.AddItemAsync(userId, dto);
            return Ok(new { message = "Producto agregado al carrito" });
        }

        [HttpPatch("{productId}")]
        public async Task<IActionResult> UpdateQuantity(int productId, [FromBody] int quantity)
        {
            var userId = GetUserIdFromClaims();
            await _cartItemService.UpdateQuantityAsync(userId, productId, quantity);
            return Ok(new { message = "Cantidad actualizada" });
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var userId = GetUserIdFromClaims();
            await _cartItemService.RemoveItemAsync(userId, productId);
            return Ok(new { message = "Producto eliminado del carrito" });
        }

        private int GetUserIdFromClaims()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");
            if (claim == null)
            {
                // TEMPORAL para pruebas sin JWT
                return 1; // o cualquier ID de usuario válido
            }
            return int.Parse(claim.Value);
        }
    }
}
