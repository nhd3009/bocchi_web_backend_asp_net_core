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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<CategoryDto>> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return new ApiResponse<CategoryDto>
                {
                    Success = false,
                    Message = "Category not found",
                    StatusCode = 404
                };
            }

            return new ApiResponse<CategoryDto>
            {
                Success = true,
                Message = "Category retrieved successfully",
                Data = MapToDto(category),
                StatusCode = 200
            };
        }

        public async Task<ApiResponse<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return new ApiResponse<List<CategoryDto>>
            {
                Success = true,
                Message = "Categories retrieved successfully",
                Data = categories.Select(MapToDto).ToList(),
                StatusCode = 200
            };
        }

        public async Task<ApiResponse<CategoryDto>> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            if (await _unitOfWork.Categories.ExistsByNameAsync(createCategoryDto.Name))
            {
                return new ApiResponse<CategoryDto>
                {
                    Success = false,
                    Message = "Category with this name already exists",
                    StatusCode = 400
                };
            }

            var category = new Category
            {
                Name = createCategoryDto.Name,
                Description = createCategoryDto.Description
            };

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<CategoryDto>
            {
                Success = true,
                Message = "Category created successfully",
                Data = MapToDto(category),
                StatusCode = 201
            };
        }

        public async Task<ApiResponse<CategoryDto>> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return new ApiResponse<CategoryDto>
                {
                    Success = false,
                    Message = "Category not found",
                    StatusCode = 404
                };
            }

            if (category.Name != updateCategoryDto.Name &&
                await _unitOfWork.Categories.ExistsByNameAsync(updateCategoryDto.Name))
            {
                return new ApiResponse<CategoryDto>
                {
                    Success = false,
                    Message = "Category with this name already exists",
                    StatusCode = 400
                };
            }

            category.Name = updateCategoryDto.Name;
            category.Description = updateCategoryDto.Description;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<CategoryDto>
            {
                Success = true,
                Message = "Category updated successfully",
                Data = MapToDto(category),
                StatusCode = 200
            };
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Category not found",
                    StatusCode = 404
                };
            }

            _unitOfWork.Categories.Remove(category);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Category deleted successfully",
                Data = true,
                StatusCode = 200
            };
        }

        private CategoryDto MapToDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt,
                CreatedBy = category.CreatedBy,
                UpdatedAt = category.UpdatedAt,
                UpdatedBy = category.UpdatedBy
            };
        }
    }
}