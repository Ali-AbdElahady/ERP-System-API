
using ERP_System.Core.Entities;
using ERP_System.Core.Specifications;

namespace ERP_System.Core.Specification.DepartmentSpecs
{
    public class DepartmentsFilterCount : BaseSpecification<Department>
    {
        public DepartmentsFilterCount(EntitySpecParams Params) : base(D =>
            string.IsNullOrEmpty(Params.Search) || D.DepartmentName.ToLower().Contains(Params.Search))
        {

        }
    }
}
