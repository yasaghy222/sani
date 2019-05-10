using DataLayer;
using ServiceLayer.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Areas;

namespace ServiceLayer.Core.Repositories
{
    public class ServiceCategoryRepository : GenericRepository<tblServiceCategory>, IServiceCategoryRepository
    {
        public ServiceCategoryRepository(SaniDBEntities context) : base(context) { }

        List<ServiceCategory.ViewServiceCategory> viewServiceCategories;
        List<ServiceCategory.ServiceCatSelect> serviceSelects;

        public List<ServiceCategory.ViewServiceCategory> GetServicesCatList()
        {

            viewServiceCategories = (from i in Context.tblServiceCategory
                                     join j in Context.tblServiceCategory
                                     on i.pid equals j.id
                                     select new ServiceCategory.ViewServiceCategory
                                     {
                                         id = i.id,
                                         pid = i.pid,
                                         title = i.title,
                                         parent = (j.title == i.title) ? "" : j.title,
                                         count = i.tblService.Count
                                     })
                                    .ToList();

            return viewServiceCategories;
        }

        public List<ServiceCategory.ServiceCatSelect> SelectServiceCat()
        {

            serviceSelects = (from i in Context.tblServiceCategory
                                     join j in Context.tblServiceCategory
                                     on i.pid equals j.id
                                     select new ServiceCategory.ServiceCatSelect
                                     {
                                         id = i.id,
                                         pid = i.pid,
                                         title = i.title,
                                     })
                                    .ToList();

            return serviceSelects;
        }


        public SaniDBEntities SaniDBEntities => Context as SaniDBEntities;
    }
}
