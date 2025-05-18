using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bocchiwebbackend.DTOs
{
    public class CreateOrderDto
    {
        public string ShippingAddress { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
    }
}