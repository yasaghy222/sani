using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Areas;
using ViewModel.General;

namespace ServiceLayer.Core.IRepositories
{
    public interface IFactorRepository : IGenericRepository<tblFactor>
    {
        Task<List<Factor.ViewTblFactor>> GetOrdersAsync(Common.PageTableVariable pageTableVariable);
        Task<List<Factor.ViewTblFactor>> FindOrder(string FillterBy);
    }
}
