using ServiceLayer.Core.IRepositories;
using DataLayer;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using ViewModel.General;
using ViewModel.Areas;

namespace ServiceLayer.Core.Repositories
{
    public class ServiceRepository : GenericRepository<tblService>, IServiceRepository
    {
        public ServiceRepository(SaniDBEntities context) : base(context) { }
        private List<Services.ViewServicesList> ViewServiceLists;
        public async Task<List<Services.ViewServicesList>> GetServicesAsync(Common.PageTableVariable pageTableVariable)
        {
            ViewServiceLists = (from item in Context.tblService
                                select new Services.ViewServicesList
                                {
                                    id = item.id,
                                    title = item.title,
                                    unit = item.unit,
                                    catName = item.tblServiceCategory.title,
                                    description = item.description,
                                    status = item.status
                                })
                                .OrderByDescending(item => item.title)
                                .Skip((pageTableVariable.PageIndex - 1) * pageTableVariable.PageSize)
                                .Take(pageTableVariable.PageSize)
                                .ToList();

            switch (pageTableVariable.OrderBy)
            {
                case 0:
                    if (pageTableVariable.OrderType)
                        ViewServiceLists = ViewServiceLists.OrderBy(item => item.title).ToList();
                    else
                        ViewServiceLists = ViewServiceLists.OrderByDescending(item => item.title).ToList();
                    break;
                case 1:
                    if (pageTableVariable.OrderType)
                        ViewServiceLists = ViewServiceLists.OrderBy(item => item.unit).ToList();
                    else
                        ViewServiceLists = ViewServiceLists.OrderByDescending(item => item.unit).ToList();
                    break;
                case 2:
                    if (pageTableVariable.OrderType)
                        ViewServiceLists.OrderBy(item => item.catName).ToList();
                    else
                        ViewServiceLists.OrderByDescending(item => item.catName).ToList();
                    break;
                case 3:
                    if (pageTableVariable.OrderType)
                        ViewServiceLists = ViewServiceLists.OrderBy(item => item.description).ToList();
                    else
                        ViewServiceLists = ViewServiceLists.OrderByDescending(item => item.description).ToList();
                    break;
                case 4:
                    if (pageTableVariable.OrderType)
                        ViewServiceLists = ViewServiceLists.OrderBy(item => item.status).ToList();
                    else
                        ViewServiceLists = ViewServiceLists.OrderByDescending(item => item.status).ToList();
                    break;
                default:
                    if (pageTableVariable.OrderType)
                        ViewServiceLists = ViewServiceLists.OrderBy(item => item.title).ToList();
                    else
                        ViewServiceLists = ViewServiceLists.OrderByDescending(item => item.title).ToList();
                    break;
            }

            return await Task.Run(() => ViewServiceLists);
        }
        public List<Services.ViewServicesList> FindServices(string FillterBy)
        {
            ViewServiceLists = (from item in Context.tblService
                                where item.title.Contains(FillterBy)
                                select new Services.ViewServicesList
                                {
                                    id = item.id,
                                    title = item.title,
                                    unit = item.unit,
                                    catName = item.tblServiceCategory.title,
                                    description = item.description,
                                    status = item.status
                                })
                                .OrderByDescending(item => item.title)
                                .ToList();

            return ViewServiceLists;
        }

        public SaniDBEntities SaniDBEntities => Context as SaniDBEntities;

    }
}
