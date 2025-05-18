using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bocchiwebbackend.DTOs
{
    public class CartDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<CartItemDto> Items { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}