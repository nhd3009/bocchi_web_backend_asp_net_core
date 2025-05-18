using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using bocchiwebbackend.DTOs;
using bocchiwebbackend.Models;
using bocchiwebbackend.Models.Common;
using bocchiwebbackend.Repositories;

namespace bocchiwebbackend.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public UserService(IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }


        public async Task<ApiResponse<UserDto>> RegisterAsync(RegisterDto registerDto)
        {
            if (await _unitOfWork.Users.ExistsAsync(u => u.Username == registerDto.Username))
            {
                var errors = new Dictionary<string, List<string>>{
                    { "Username", new List<string> { "Username already exists" } }
                };
                return ApiResponse<UserDto>.ErrorResponse("Username already exists", errors);
            }

            if (await _unitOfWork.Users.ExistsAsync(u => u.Email == registerDto.Email))
            {
                var errors = new Dictionary<string, List<string>>{
                    { "Email", new List<string> { "Email already exists" } }
                };
                return ApiResponse<UserDto>.ErrorResponse("Email already exists", errors);
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = HashPassword(registerDto.Password),
                FirstName = registerDto.FirstName ?? string.Empty,
                LastName = registerDto.LastName ?? string.Empty,
                Role = "Customer",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var userDto = MapToUserDto(user);

            return ApiResponse<UserDto>.CreatedResponse(userDto, "User registered successfully");
        }

        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(loginDto.Username);

            if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return ApiResponse<AuthResponseDto>.ErrorResponse("Invalid username or password", "INVALID_CREDENTIALS");
            }

            if (!user.IsActive)
            {
                return ApiResponse<AuthResponseDto>.ErrorResponse("Account is disabled", "ACCOUNT_DISABLED");
            }

            user.LastLogin = DateTime.UtcNow;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);

            var authResponse = new AuthResponseDto
            {
                Token = token,
                User = MapToUserDto(user),
                Expiration = DateTime.UtcNow.AddMinutes(120)
            };

            return ApiResponse<AuthResponseDto>.SuccessResponse(authResponse, "Login successful");
        }

        public async Task<ApiResponse<UserDto>> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
            {
                return ApiResponse<UserDto>.NotFoundResponse("User not found", "USER");
            }

            var userDto = MapToUserDto(user);
            return ApiResponse<UserDto>.SuccessResponse(userDto);
        }

        public async Task<ApiResponse<PagedList<UserDto>>> GetAllAsync(int page = 1, int pageSize = 10)
        {
            var totalCount = await _unitOfWork.Users.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            page = Math.Max(1, Math.Min(page, totalPages == 0 ? 1 : totalPages));

            var usersQuery = (await _unitOfWork.Users.GetAllAsync())
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            var userDtos = usersQuery.Select(u => MapToUserDto(u)).ToList();
            var pagedList = new PagedList<UserDto>
            {
                Items = userDtos,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page,
                TotalPages = totalPages
            };
            return ApiResponse<PagedList<UserDto>>.SuccessResponse(pagedList);
        }

        public async Task<ApiResponse<UserDto>> UpdateAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
            {
                return ApiResponse<UserDto>.NotFoundResponse("User not found", "USER");
            }

            if (!string.IsNullOrEmpty(updateUserDto.Email) &&
                updateUserDto.Email != user.Email &&
                await _unitOfWork.Users.ExistsAsync(u => u.Email == updateUserDto.Email))
            {
                var errors = new Dictionary<string, List<string>>
        {
            { "Email", new List<string> { "Email already exists" } }
        };
                return ApiResponse<UserDto>.ErrorResponse("Email already exists", errors);
            }

            if (!string.IsNullOrEmpty(updateUserDto.Email))
                user.Email = updateUserDto.Email;

            if (!string.IsNullOrEmpty(updateUserDto.FirstName))
                user.FirstName = updateUserDto.FirstName;

            if (!string.IsNullOrEmpty(updateUserDto.LastName))
                user.LastName = updateUserDto.LastName;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            var userDto = MapToUserDto(user);
            return ApiResponse<UserDto>.SuccessResponse(userDto, "User updated successfully");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
            {
                return ApiResponse<bool>.NotFoundResponse("User not found", "USER");
            }

            _unitOfWork.Users.Remove(user);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.NoContentResponse("User deleted successfully");
        }

        #region Helper Methods
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }

        private UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }
        #endregion
    }
}