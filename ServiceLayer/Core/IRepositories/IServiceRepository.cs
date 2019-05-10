using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer;
using ViewModel.Areas;
using ViewModel.General;

namespace ServiceLayer.Core.IRepositories
{
    public interface IServiceRepository : IGenericRepository<tblService>
    {
        Task<List<Services.ViewServicesList>> GetServicesAsync(Common.PageTableVariable pageTableVariable);

        List<Services.ViewServicesList> FindServices(string FillterBy);
    }
}
