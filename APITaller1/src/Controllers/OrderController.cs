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
        private readonly PdfService _pdfService;

        public OrderController(OrderService orderService, PdfService pdfService)
        {
            _orderService = orderService;
            _pdfService = pdfService;
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

        [HttpPost("filter")]
        [Authorize]
        public async Task<ActionResult<List<OrderDto>>> FilterOrders([FromBody] OrderFilterDto filter)
        {
            var userId = GetUserIdFromClaims();
            var orders = await _orderService.GetFilteredOrdersAsync(userId, filter);
            return Ok(orders);
        }

        [HttpGet("{id}/pdf")]
        [Authorize]
        public async Task<IActionResult> DownloadOrderPdf(int id)
        {
            var userId = GetUserIdFromClaims();
            var order = await _orderService.GetOrderByIdAsync(userId, id);

            if (order == null)
                return NotFound("Pedido no encontrado.");

            var pdfBytes = _pdfService.GenerateOrderPdf(order);

            return File(pdfBytes, "application/pdf", $"pedido_{id}.pdf");
        }

    }
}
