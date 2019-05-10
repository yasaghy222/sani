using DataLayer;
using ServiceLayer.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Areas;
using Utility;

namespace ServiceLayer.Core.Repositories
{
    public class ServiceAssignRepository : GenericRepository<tblBranchService>, IServiceAssignRepository
    {
        public ServiceAssignRepository(SaniDBEntities context) : base(context) { }

        private List<Services.ViewServiceAssignList> ViewServiceAssinList;

        public List<Services.ViewServiceAssignList> GetServiceAssign(Guid id)
        {
            ViewServiceAssinList = (from item in Context.tblBranchService
                                    where item.serviceId == id
                                    select new Services.ViewServiceAssignList
                                    {
                                        bCode = item.bCode,
                                        bTitle = item.tblBranch.title,
                                        serviceId = item.serviceId,
                                        price = item.price.ToString()
                                    })
                                    .OrderByDescending(item => item.bTitle)
                                    .ToList();


            return ViewServiceAssinList;
        }

        public SaniDBEntities SaniDBEntities => Context as SaniDBEntities;
    }
}


