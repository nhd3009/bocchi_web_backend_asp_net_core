using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using bocchiwebbackend.Models;

namespace bocchiwebbackend.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}