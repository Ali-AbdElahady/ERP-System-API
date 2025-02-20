using ERP_System.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System.Service.DTO
{
    public class EmployeeDTO : CreateUpdateEmployeeDto
    {
        public int Id { get; set; }
    }
}
