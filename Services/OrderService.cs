using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.DTOs;
using bocchiwebbackend.Models;
using bocchiwebbackend.Models.Common;
using bocchiwebbackend.Repositories;

namespace bocchiwebbackend.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;

        public OrderService(IUnitOfWork unitOfWork, ICartService cartService)
        {
            _unitOfWork = unitOfWork;
            _cartService = cartService;
        }

        public async Task<ApiResponse<OrderDto>> GetOrderByIdAsync(int orderId, int userId)
        {
            var order = await _unitOfWork.Orders.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return ApiResponse<OrderDto>.NotFoundResponse("Order not found", "ORDER");
            }

            if (order.UserId != userId)
            {
                return ApiResponse<OrderDto>.ErrorResponse("You don't have permission to view this order", "FORBIDDEN", 403);
            }

            return ApiResponse<OrderDto>.SuccessResponse(MapToDto(order));
        }

        public async Task<ApiResponse<List<OrderDto>>> GetUserOrdersAsync(int userId)
        {
            var orders = await _unitOfWork.Orders.GetOrdersByUserIdAsync(userId);
            return ApiResponse<List<OrderDto>>.SuccessResponse(orders.Select(MapToDto).ToList());
        }

        public async Task<ApiResponse<OrderDto>> CreateOrderAsync(int userId, CreateOrderDto createOrderDto)
        {
            var cartResponse = await _cartService.GetCartAsync(userId);
            if (!cartResponse.Success)
            {
                return ApiResponse<OrderDto>.ErrorResponse("Failed to get cart", "CART_ERROR");
            }

            var cart = cartResponse.Data;
            if (cart.Items.Count == 0)
            {
                return ApiResponse<OrderDto>.ErrorResponse("Cart is empty", "EMPTY_CART");
            }

            var order = new Order
            {
                UserId = userId,
                ShippingAddress = createOrderDto.ShippingAddress,
                RecipientName = createOrderDto.RecipientName,
                RecipientPhone = createOrderDto.RecipientPhone,
                Status = "Pending",
                OrderDate = DateTime.UtcNow,
                TotalAmount = cart.TotalPrice,
                OrderItems = cart.Items.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.ProductPrice
                }).ToList()
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            await _cartService.ClearCartAsync(userId);

            return ApiResponse<OrderDto>.CreatedResponse(MapToDto(order), "Order created successfully");
        }

        public async Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _unitOfWork.Orders.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return ApiResponse<OrderDto>.NotFoundResponse("Order not found", "ORDER");
            }

            if (order.Status == "Completed" || order.Status == "Cancelled")
            {
                return ApiResponse<OrderDto>.ErrorResponse("Cannot update completed or cancelled order", "INVALID_STATUS");
            }

            order.Status = status;
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<OrderDto>.SuccessResponse(MapToDto(order), "Order status updated successfully");
        }

        public async Task<ApiResponse<bool>> CancelOrderAsync(int orderId, int userId)
        {
            var order = await _unitOfWork.Orders.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return ApiResponse<bool>.NotFoundResponse("Order not found", "ORDER");
            }

            if (order.UserId != userId)
            {
                return ApiResponse<bool>.ErrorResponse("You don't have permission to view this order", "FORBIDDEN", 403);
            }

            if (order.Status == "Completed" || order.Status == "Cancelled")
            {
                return ApiResponse<bool>.ErrorResponse("Cannot cancel completed or already cancelled order", "INVALID_STATUS");
            }

            order.Status = "Cancelled";
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Order cancelled successfully");
        }

        private OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                ShippingAddress = order.ShippingAddress,
                RecipientName = order.RecipientName,
                RecipientPhone = order.RecipientPhone,
                Status = order.Status,
                OrderDate = order.OrderDate,
                Items = order.OrderItems.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    TotalPrice = item.Price * item.Quantity
                }).ToList()
            };
        }
    }
}