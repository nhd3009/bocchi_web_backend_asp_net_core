using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bocchiwebbackend.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
        public DateTime Expiration { get; set; }
    }
}