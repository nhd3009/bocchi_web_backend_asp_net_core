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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public ProductService(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<ApiResponse<ProductDto>> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetProductWithCategoryAsync(id);
            if (product == null)
            {
                return ApiResponse<ProductDto>.NotFoundResponse("Product not found", "PRODUCT");
            }

            return ApiResponse<ProductDto>.SuccessResponse(MapToDto(product));
        }

        public async Task<ApiResponse<List<ProductDto>>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            var productDtos = products.Select(MapToDto).ToList();
            return ApiResponse<List<ProductDto>>.SuccessResponse(productDtos);
        }

        public async Task<ApiResponse<ProductDto>> CreateAsync(CreateProductDto createProductDto, IFormFile image)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(createProductDto.CategoryId);
            if (category == null)
            {
                return ApiResponse<ProductDto>.NotFoundResponse("Category not found", "CATEGORY");
            }

            if (await _unitOfWork.Products.ExistsByNameAsync(createProductDto.Name))
            {
                var errors = new Dictionary<string, List<string>>
                {
                    { "Name", new List<string> { "Product with this name already exists" } }
                };
                return ApiResponse<ProductDto>.ErrorResponse("Product name already exists", errors);
            }

            string imageUrl = null;
            if (image != null)
            {
                try
                {
                    imageUrl = await _fileService.UploadImageAsync(image);
                }
                catch (ArgumentException ex)
                {
                    var errors = new Dictionary<string, List<string>>
                    {
                        { "Image", new List<string> { ex.Message } }
                    };
                    return ApiResponse<ProductDto>.ErrorResponse("Invalid image", errors);
                }
            }

            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity,
                CategoryId = createProductDto.CategoryId,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<ProductDto>.CreatedResponse(MapToDto(product), "Product created successfully");
        }

        public async Task<ApiResponse<ProductDto>> UpdateAsync(int id, UpdateProductDto updateProductDto, IFormFile image)
        {
            var product = await _unitOfWork.Products.GetProductWithCategoryAsync(id);
            if (product == null)
            {
                return ApiResponse<ProductDto>.NotFoundResponse("Product not found", "PRODUCT");
            }

            if (product.CategoryId != updateProductDto.CategoryId)
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(updateProductDto.CategoryId);
                if (category == null)
                {
                    return ApiResponse<ProductDto>.NotFoundResponse("Category not found", "CATEGORY");
                }
            }

            if (product.Name != updateProductDto.Name &&
                await _unitOfWork.Products.ExistsByNameAsync(updateProductDto.Name))
            {
                var errors = new Dictionary<string, List<string>>
                {
                    { "Name", new List<string> { "Product with this name already exists" } }
                };
                return ApiResponse<ProductDto>.ErrorResponse("Product name already exists", errors);
            }

            if (image != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(product.ImageUrl))
                    {
                        _fileService.DeleteImage(product.ImageUrl);
                    }

                    product.ImageUrl = await _fileService.UploadImageAsync(image);
                }
                catch (ArgumentException ex)
                {
                    var errors = new Dictionary<string, List<string>>
                    {
                        { "Image", new List<string> { ex.Message } }
                    };
                    return ApiResponse<ProductDto>.ErrorResponse("Invalid image", errors);
                }
            }

            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;
            product.StockQuantity = updateProductDto.StockQuantity;
            product.CategoryId = updateProductDto.CategoryId;
            product.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<ProductDto>.SuccessResponse(MapToDto(product), "Product updated successfully");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return ApiResponse<bool>.NotFoundResponse("Product not found", "PRODUCT");
            }

            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                _fileService.DeleteImage(product.ImageUrl);
            }

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.NoContentResponse("Product deleted successfully");
        }

        private ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                CreatedAt = product.CreatedAt,
                CreatedBy = product.CreatedBy,
                UpdatedAt = product.UpdatedAt,
                UpdatedBy = product.UpdatedBy
            };
        }
    }
}