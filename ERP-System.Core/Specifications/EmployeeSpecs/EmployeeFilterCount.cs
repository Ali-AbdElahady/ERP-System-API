using ERP_System.Core.Entities;
using ERP_System.Core.Specification.EmployeeSpecs;

namespace ERP_System.Core.Specifications.EmployeeSpec
{
    public class EmployeeFilterCount : BaseSpecification<Employee>
    {
        public EmployeeFilterCount(EmployeeSpecParams Params) : base(E =>
        (Params.DepartmentId == 0 || E.DepartmentId == Params.DepartmentId) &&
        (String.IsNullOrWhiteSpace(Params.Search) || E.FirstName.ToLower().Contains(Params.Search.ToLower()) || E.FirstName.ToLower().Contains(Params.Search.ToLower())) &&
        (String.IsNullOrWhiteSpace(Params.JobTitle) || E.JobTitle.ToLower().Contains(Params.JobTitle.ToLower())))
        {

        }
    }
}
