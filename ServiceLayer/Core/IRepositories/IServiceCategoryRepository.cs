using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Areas;

namespace ServiceLayer.Core.IRepositories
{
    public interface IServiceCategoryRepository : IGenericRepository<tblServiceCategory>
    {
        List<ServiceCategory.ViewServiceCategory> GetServicesCatList();

        List<ServiceCategory.ServiceCatSelect> SelectServiceCat();
    }
}
