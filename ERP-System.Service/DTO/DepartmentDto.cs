using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_System.Service.DTO
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        [Required, MaxLength(100)]
        public string DepartmentName { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public int? ManagerId { get; set; }
    }
}
