using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.DTOs;
using bocchiwebbackend.Models.Common;

namespace bocchiwebbackend.Services
{
    public interface ICategoryService
    {
        Task<ApiResponse<CategoryDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<CategoryDto>>> GetAllAsync();
        Task<ApiResponse<CategoryDto>> CreateAsync(CreateCategoryDto createCategoryDto);
        Task<ApiResponse<CategoryDto>> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}