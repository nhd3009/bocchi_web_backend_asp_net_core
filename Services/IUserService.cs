using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.DTOs;
using bocchiwebbackend.Models.Common;

namespace bocchiwebbackend.Services
{
    public interface IUserService
    {
        Task<ApiResponse<UserDto>> RegisterAsync(RegisterDto registerDto);
        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ApiResponse<UserDto>> GetByIdAsync(int id);
        Task<ApiResponse<PagedList<UserDto>>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<ApiResponse<UserDto>> UpdateAsync(int id, UpdateUserDto updateUserDto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}