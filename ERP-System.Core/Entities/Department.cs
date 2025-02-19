using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_System.Core.Entities
{
    public class Department
    {
        public int DepartmentId { get; set; }
        [Required, MaxLength(100)]
        public string DepartmentName { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public int? ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public virtual Employee Manager { get; set; }
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    }
}
