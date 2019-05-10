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
    public interface IForceRepository :IGenericRepository<tblForce>
    {
        Task<List<Force.ViewTblForce>> GetForceAsync(Common.PageTableVariable pageTableVariable);
        List<Force.ViewTblForce> FindForce(string FillterBy);
        List<Force.SelectAForce> SelectAForce();
    }
}
