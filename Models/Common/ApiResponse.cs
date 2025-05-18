using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bocchiwebbackend.Models.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public object? Errors { get; set; }
        public int StatusCode { get; set; }
        public Dictionary<string, object>? Meta { get; set; }

        // Success response with data
        public static ApiResponse<T> SuccessResponse(T data, string message = "Operation successful", Dictionary<string, object>? meta = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = 200,
                Meta = meta
            };
        }

        // Success response without data
        public static ApiResponse<T> SuccessResponse(string message = "Operation successful", Dictionary<string, object>? meta = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                StatusCode = 200,
                Meta = meta
            };
        }

        public static ApiResponse<T> ErrorResponse(string message = "Operation failed", Dictionary<string, List<string>>? validationErrors = null, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = validationErrors,
                StatusCode = statusCode
            };
        }

        public static ApiResponse<T> ErrorResponse(string message = "Operation failed", string errorCode = "ERROR", int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errorCode,
                StatusCode = statusCode
            };
        }

        public static ApiResponse<T> NotFoundResponse(string message = "Resource not found", string resourceType = "")
        {
            var errorCode = string.IsNullOrEmpty(resourceType) ? "NOT_FOUND" : $"{resourceType.ToUpper()}_NOT_FOUND";

            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errorCode,
                StatusCode = 404
            };
        }

        public static ApiResponse<T> UnauthorizedResponse(string message = "Unauthorized access")
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = "UNAUTHORIZED",
                StatusCode = 401
            };
        }

        public static ApiResponse<T> CreatedResponse(T data, string message = "Resource created successfully")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = 201
            };
        }

        public static ApiResponse<T> NoContentResponse(string message = "Operation successful")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                StatusCode = 204
            };
        }

        public void AddMeta(string key, object value)
        {
            Meta ??= new Dictionary<string, object>();
            Meta[key] = value;
        }
    }
}