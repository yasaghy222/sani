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
    public interface IElectricShopRepository : IGenericRepository<tblElctricShop>
    {
        Task<List<EShop.ViewTblEShop>> GetEShopAsync(Common.PageTableVariable pageTableVariable);
        List<EShop.ViewTblEShop> FindEShop(string FillterBy);
    }
}
