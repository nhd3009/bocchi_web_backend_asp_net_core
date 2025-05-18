using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public static class ControllerExtensions
{
    public static ActionResult<ApiResponse<T>> ApiOk<T>(this ControllerBase controller, T data, string message = "Operation successful", Dictionary<string, object>? meta = null)
    {
        var response = ApiResponse<T>.SuccessResponse(data, message, meta);
        return controller.Ok(response);
    }

    public static ActionResult<ApiResponse<T>> ApiOk<T>(this ControllerBase controller, string message = "Operation successful", Dictionary<string, object>? meta = null)
    {
        var response = ApiResponse<T>.SuccessResponse(message, meta);
        return controller.Ok(response);
    }

    public static ActionResult<ApiResponse<T>> ApiBadRequest<T>(this ControllerBase controller, string message = "Bad request", Dictionary<string, List<string>>? validationErrors = null)
    {
        var response = ApiResponse<T>.ErrorResponse(message, validationErrors, 400);
        return controller.BadRequest(response);
    }

    public static ActionResult<ApiResponse<T>> ApiBadRequest<T>(this ControllerBase controller, string message = "Bad request", string errorCode = "BAD_REQUEST")
    {
        var response = ApiResponse<T>.ErrorResponse(message, errorCode, 400);
        return controller.BadRequest(response);
    }

    public static ActionResult<ApiResponse<T>> ApiNotFound<T>(this ControllerBase controller, string message = "Resource not found", string resourceType = "")
    {
        var response = ApiResponse<T>.NotFoundResponse(message, resourceType);
        return controller.NotFound(response);
    }

    public static ActionResult<ApiResponse<T>> ApiUnauthorized<T>(this ControllerBase controller, string message = "Unauthorized access")
    {
        var response = ApiResponse<T>.UnauthorizedResponse(message);
        return controller.StatusCode(401, response);
    }

    public static ActionResult<ApiResponse<T>> ApiCreated<T>(this ControllerBase controller, T data, string message = "Resource created successfully")
    {
        var response = ApiResponse<T>.CreatedResponse(data, message);
        return controller.StatusCode(201, response);
    }

    public static ActionResult<ApiResponse<T>> ApiNoContent<T>(this ControllerBase controller, string message = "Operation successful")
    {
        var response = ApiResponse<T>.NoContentResponse(message);
        return controller.StatusCode(204, response);
    }

    public static ActionResult<ApiResponse<T>> ApiModelStateErrors<T>(this ControllerBase controller, ModelStateDictionary modelState, string message = "Validation failed")
    {
        var errors = new Dictionary<string, List<string>>();

        foreach (var key in modelState.Keys)
        {
            if (modelState[key]?.Errors.Count > 0)
            {
                errors[key] = modelState[key]!.Errors.Select(e => e.ErrorMessage).ToList();
            }
        }

        return controller.ApiBadRequest<T>(message, errors);
    }
}