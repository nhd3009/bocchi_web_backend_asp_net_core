using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Models.Common;

namespace bocchiwebbackend.Models
{
    public class Product : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}