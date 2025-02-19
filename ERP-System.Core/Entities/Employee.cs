using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_System.Core.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        [Required, MaxLength(15)]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public DateTime HireDate { get; set; }
        [Required, MaxLength(50)]
        public string JobTitle { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
        [ForeignKey("DepartmentId")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public int? ManagerId { get; set; } // Nullable because not every employee has a manager
        [ForeignKey("ManagerId")]
        public virtual Employee Manager { get; set; }  // Self-referencing FK
        public virtual ICollection<Employee> Subordinates { get; set; } = new List<Employee>();
        public bool IsActive { get; set; } = true;
    }
}
