using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using bocchiwebbackend.DTOs;
using bocchiwebbackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bocchiwebbackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }
            return int.Parse(userIdClaim.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            try
            {
                var userId = GetUserId();
                var response = await _orderService.GetUserOrdersAsync(userId);
                return StatusCode(response.StatusCode, response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var userId = GetUserId();
                var response = await _orderService.GetOrderByIdAsync(id, userId);
                return StatusCode(response.StatusCode, response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                var userId = GetUserId();
                var response = await _orderService.CreateOrderAsync(userId, createOrderDto);
                return StatusCode(response.StatusCode, response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto dto)
        {
            var response = await _orderService.UpdateOrderStatusAsync(id, dto.Status);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            try
            {
                var userId = GetUserId();
                var response = await _orderService.CancelOrderAsync(id, userId);
                return StatusCode(response.StatusCode, response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
        }
    }
}