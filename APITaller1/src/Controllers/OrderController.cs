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
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder()
        {
            var userId = GetUserIdFromClaims();
            var orderDto = await _orderService.CreateOrderAsync(userId);
            return Ok(orderDto);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            var userId = GetUserIdFromClaims();
            var orders = await _orderService.GetOrdersByUserAsync(userId);
            return Ok(orders);
        }

        private int GetUserIdFromClaims()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                throw new UnauthorizedAccessException("Usuario no autenticado");
            }
            return int.Parse(claim.Value);
        }
    }
}
