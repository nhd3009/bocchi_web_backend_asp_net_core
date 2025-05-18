using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bocchiwebbackend.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}