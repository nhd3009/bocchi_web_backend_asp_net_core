using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Models.Common;

namespace bocchiwebbackend.Models
{
    public class Order : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public Decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}