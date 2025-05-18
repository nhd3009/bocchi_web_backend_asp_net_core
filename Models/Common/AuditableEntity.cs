using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bocchiwebbackend.Models.Common
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}