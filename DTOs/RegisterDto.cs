using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bocchiwebbackend.DTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}