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
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
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
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var userId = GetUserId();
                var response = await _cartService.GetCartAsync(userId);
                return StatusCode(response.StatusCode, response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddCartItemDto addCartItemDto)
        {
            try
            {
                var userId = GetUserId();
                var response = await _cartService.AddItemToCartAsync(userId, addCartItemDto);
                return StatusCode(response.StatusCode, response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
        }

        [HttpPut("items/{productId}")]
        public async Task<IActionResult> UpdateCartItemQuantity(int productId, [FromBody] UpdateCartItemQuantityDto dto)
        {
            try
            {
                var userId = GetUserId();
                var response = await _cartService.UpdateCartItemQuantityAsync(userId, productId, dto.Quantity);
                return StatusCode(response.StatusCode, response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
        }

        [HttpDelete("items/{productId}")]
        public async Task<IActionResult> RemoveItemFromCart(int productId)
        {
            try
            {
                var userId = GetUserId();
                var response = await _cartService.RemoveItemFromCartAsync(userId, productId);
                return StatusCode(response.StatusCode, response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = GetUserId();
                var response = await _cartService.ClearCartAsync(userId);
                return StatusCode(response.StatusCode, response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
        }
    }
}