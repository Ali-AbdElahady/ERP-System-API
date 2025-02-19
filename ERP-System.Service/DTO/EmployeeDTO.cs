using ERP_System.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Service.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }
        [Required, MaxLength(15),Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public DateTime HireDate { get; set; }
        [Required, MaxLength(50)]
        public string JobTitle { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        public int? ManagerId { get; set; } // Nullable because not every employee has a manager
        public bool IsActive { get; set; } = true;
    }
}
