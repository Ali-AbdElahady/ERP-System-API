

using ERP_System.Core.Entities;
using ERP_System.Core.Specifications;

namespace ERP_System.Core.Specification.EmployeeSpecs
{
    public class EmployeeSpecification : BaseSpecification<Employee>
    {
        public EmployeeSpecification(EmployeeSpecParams Params):base(E=>
        (Params.DepartmentId == 0 || E.DepartmentId == Params.DepartmentId) &&
        (String.IsNullOrWhiteSpace(Params.Search) || E.FirstName.ToLower().Contains(Params.Search.ToLower()) || E.FirstName.ToLower().Contains(Params.Search.ToLower())) &&
        (String.IsNullOrWhiteSpace(Params.JobTitle) || E.JobTitle.ToLower().Contains(Params.JobTitle.ToLower()) )
        )
        {
            AddIncludes(E => E.Department);
            ApplyPagination((Params.pageNumber - 1) * Params.PageSize, Params.PageSize);
        }

        public EmployeeSpecification(int id) : base(D => D.Id == id)
        {
            Includes.Add(D => D.Department);
        }
    }
}
