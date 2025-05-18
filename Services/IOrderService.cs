using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.DTOs;
using bocchiwebbackend.Models.Common;

namespace bocchiwebbackend.Services
{
    public interface IOrderService
    {
        Task<ApiResponse<OrderDto>> GetOrderByIdAsync(int orderId, int userId);
        Task<ApiResponse<List<OrderDto>>> GetUserOrdersAsync(int userId);
        Task<ApiResponse<OrderDto>> CreateOrderAsync(int userId, CreateOrderDto createOrderDto);
        Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(int orderId, string status);
        Task<ApiResponse<bool>> CancelOrderAsync(int orderId, int userId);
    }
}