using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Models.Common;

namespace bocchiwebbackend.Models
{
    public class Category : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}