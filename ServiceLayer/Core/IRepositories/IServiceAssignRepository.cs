using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Areas;

namespace ServiceLayer.Core.IRepositories
{
    public interface IServiceAssignRepository : IGenericRepository<tblBranchService>
    {
        List<Services.ViewServiceAssignList> GetServiceAssign(Guid id);
    }
}
