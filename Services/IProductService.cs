using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.DTOs;
using bocchiwebbackend.Models.Common;

namespace bocchiwebbackend.Services
{
    public interface IProductService
    {
        Task<ApiResponse<ProductDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<ProductDto>>> GetAllAsync();
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<ProductDto>> CreateAsync(CreateProductDto createProductDto, IFormFile image);
        Task<ApiResponse<ProductDto>> UpdateAsync(int id, UpdateProductDto updateProductDto, IFormFile image);
    }
}