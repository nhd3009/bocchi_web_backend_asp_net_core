using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.DTOs;
using bocchiwebbackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bocchiwebbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _categoryService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto createCategoryDto)
        {
            var response = await _categoryService.CreateAsync(createCategoryDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            var response = await _categoryService.UpdateAsync(id, updateCategoryDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _categoryService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}