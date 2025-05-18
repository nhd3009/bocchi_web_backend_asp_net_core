using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.DTOs;
using bocchiwebbackend.Models.Common;
using bocchiwebbackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bocchiwebbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ProductDto>>>> GetAll()
        {
            var response = await _productService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> GetById(int id)
        {
            var response = await _productService.GetByIdAsync(id);
            if (!response.Success)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductDto>>> Create([FromForm] CreateProductDto createProductDto)
        {
            var image = Request.Form.Files.FirstOrDefault();
            var response = await _productService.CreateAsync(createProductDto, image);

            if (!response.Success)
            {
                return StatusCode(response.StatusCode, response);
            }

            return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> Update(int id, [FromForm] UpdateProductDto updateProductDto)
        {
            var image = Request.Form.Files.FirstOrDefault();
            var response = await _productService.UpdateAsync(id, updateProductDto, image);

            if (!response.Success)
            {
                return StatusCode(response.StatusCode, response);
            }

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var response = await _productService.DeleteAsync(id);

            if (!response.Success)
            {
                return StatusCode(response.StatusCode, response);
            }

            return NoContent();
        }
    }
}