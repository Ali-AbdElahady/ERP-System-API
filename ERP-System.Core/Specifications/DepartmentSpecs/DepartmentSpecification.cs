
using ERP_System.Core.Entities;
using ERP_System.Core.Specifications;

namespace ERP_System.Core.Specification.EmployeeSpecs
{
    public class DepartmentSpecification : BaseSpecification<Department>
    {
        public DepartmentSpecification(EntitySpecParams Params):base(D=>
            (String.IsNullOrWhiteSpace(Params.Search) || D.DepartmentName.ToLower().Contains(Params.Search.ToLower()))
        )
        {
            AddIncludes(E => E.Manager);
            ApplyPagination((Params.pageNumber - 1) * Params.PageSize, Params.PageSize);
        }

        public DepartmentSpecification(int id) : base(D => D.DepartmentId == id)
        {
            AddIncludes(E => E.Manager);
        }
    }
}
