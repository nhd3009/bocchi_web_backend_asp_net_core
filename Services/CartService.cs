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
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<CartDto>> GetCartAsync(int userId)
        {
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return ApiResponse<CartDto>.NotFoundResponse("Cart not found", "CART");
            }

            return ApiResponse<CartDto>.SuccessResponse(MapToDto(cart));
        }

        public async Task<ApiResponse<CartDto>> AddItemToCartAsync(int userId, AddCartItemDto addCartItemDto)
        {
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _unitOfWork.Carts.AddAsync(cart);
                await _unitOfWork.SaveChangesAsync();
            }

            var product = await _unitOfWork.Products.GetByIdAsync(addCartItemDto.ProductId);
            if (product == null)
            {
                return ApiResponse<CartDto>.NotFoundResponse("Product not found", "PRODUCT");
            }

            var existingItem = await _unitOfWork.CartItems.GetCartItemByCartIdAndProductIdAsync(cart.Id, addCartItemDto.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += addCartItemDto.Quantity;
                _unitOfWork.CartItems.Update(existingItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = addCartItemDto.ProductId,
                    Quantity = addCartItemDto.Quantity
                };
                await _unitOfWork.CartItems.AddAsync(cartItem);
            }

            await _unitOfWork.SaveChangesAsync();
            return await GetCartAsync(userId);
        }

        public async Task<ApiResponse<CartDto>> UpdateCartItemQuantityAsync(int userId, int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return ApiResponse<CartDto>.ErrorResponse("Quantity must be greater than 0", "INVALID_QUANTITY");
            }

            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return ApiResponse<CartDto>.NotFoundResponse("Cart not found", "CART");
            }

            var cartItem = await _unitOfWork.CartItems.GetCartItemByCartIdAndProductIdAsync(cart.Id, productId);
            if (cartItem == null)
            {
                return ApiResponse<CartDto>.NotFoundResponse("Cart item not found", "CART_ITEM");
            }

            cartItem.Quantity = quantity;
            _unitOfWork.CartItems.Update(cartItem);
            await _unitOfWork.SaveChangesAsync();

            return await GetCartAsync(userId);
        }

        public async Task<ApiResponse<bool>> RemoveItemFromCartAsync(int userId, int productId)
        {
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return ApiResponse<bool>.NotFoundResponse("Cart not found", "CART");
            }

            var cartItem = await _unitOfWork.CartItems.GetCartItemByCartIdAndProductIdAsync(cart.Id, productId);
            if (cartItem == null)
            {
                return ApiResponse<bool>.NotFoundResponse("Cart item not found", "CART_ITEM");
            }

            _unitOfWork.CartItems.Remove(cartItem);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Item removed from cart");
        }

        public async Task<ApiResponse<bool>> ClearCartAsync(int userId)
        {
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return ApiResponse<bool>.NotFoundResponse("Cart not found", "CART");
            }

            var cartItems = await _unitOfWork.CartItems.GetCartItemsByCartIdAsync(cart.Id);
            foreach (var item in cartItems)
            {
                _unitOfWork.CartItems.Remove(item);
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Cart cleared successfully");
        }

        private CartDto MapToDto(Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.CartItems.Select(item => new CartItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    ProductPrice = item.Product?.Price ?? 0,
                    Quantity = item.Quantity,
                    TotalPrice = (item.Product?.Price ?? 0) * item.Quantity
                }).ToList(),
                TotalItems = cart.CartItems.Sum(item => item.Quantity),
                TotalPrice = cart.CartItems.Sum(item => (item.Product?.Price ?? 0) * item.Quantity)
            };
        }
    }
}