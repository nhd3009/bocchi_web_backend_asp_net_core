using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.DTOs;
using bocchiwebbackend.Models.Common;

namespace bocchiwebbackend.Services
{
    public interface ICartService
    {
        Task<ApiResponse<CartDto>> GetCartAsync(int userId);
        Task<ApiResponse<CartDto>> AddItemToCartAsync(int userId, AddCartItemDto addCartItemDto);
        Task<ApiResponse<CartDto>> UpdateCartItemQuantityAsync(int userId, int productId, int quantity);
        Task<ApiResponse<bool>> RemoveItemFromCartAsync(int userId, int productId);
        Task<ApiResponse<bool>> ClearCartAsync(int userId);
    }
}