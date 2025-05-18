using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Models.Common;

namespace bocchiwebbackend.Models
{
    public class Cart : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}