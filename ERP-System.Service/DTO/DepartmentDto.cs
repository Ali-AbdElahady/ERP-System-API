using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_System.Service.DTO
{
    public class DepartmentDto : CreateUpdateDepartmentDto
    {
        public int DepartmentId { get; set; }
    }
}
