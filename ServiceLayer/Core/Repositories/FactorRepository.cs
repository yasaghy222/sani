using DataLayer;
using ServiceLayer.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Areas;
using ViewModel.General;

namespace ServiceLayer.Core.Repositories
{
    public class FactorRepository : GenericRepository<tblFactor>, IFactorRepository
    {
        public FactorRepository(SaniDBEntities context) : base(context) { }

        private List<Factor.ViewTblFactor> ViewTblFactors = null;

        public async Task<List<Factor.ViewTblFactor>> GetOrdersAsync(Common.PageTableVariable pageTableVariable)
        {
            ViewTblFactors = (from item in Context.tblFactor
                              where item.status > 0
                              select new Factor.ViewTblFactor
                              {
                                  id = item.id,
                                  code = item.code,
                                  registerDate = item.registerDate,
                                  tblCustomer = item.tblCustomer,
                                  customerId = item.customerId,
                                  tblAssignToFactor = item.tblAssignToFactor,
                                  tblForce = item.tblForce,
                                  forceId = item.forceId,
                                  elctricShopId = item.elctricShopId,
                                  opratorId = item.opratorId,
                                  tblBranch = item.tblBranch,
                                  status = item.status
                              })
                         .OrderByDescending(item => item.code)
                         .Skip((pageTableVariable.PageIndex - 1) * pageTableVariable.PageSize)
                         .Take(pageTableVariable.PageSize)
                         .ToList();

            switch (pageTableVariable.OrderBy)
            {
                case 0:
                    if (pageTableVariable.OrderType)
                        ViewTblFactors = ViewTblFactors.OrderBy(item => item.code).ToList();
                    else
                        ViewTblFactors = ViewTblFactors.OrderByDescending(item => item.tblCustomer.name).ToList();
                    break;
                case 1:
                    if (pageTableVariable.OrderType)
                        ViewTblFactors = ViewTblFactors.OrderBy(item => item.registerDate).ToList();
                    else
                        ViewTblFactors = ViewTblFactors.OrderByDescending(item => item.registerDate).ToList();
                    break;
                case 2:
                    if (pageTableVariable.OrderType)
                        ViewTblFactors.OrderBy(item => item.tblCustomer.name).ToList();
                    else
                        ViewTblFactors.OrderByDescending(item => item.tblCustomer.name).ToList();
                    break;
                case 4:
                    if (pageTableVariable.OrderType)
                        ViewTblFactors = ViewTblFactors.OrderBy(item => item.tblForce.name).ToList();
                    else
                        ViewTblFactors = ViewTblFactors.OrderByDescending(item => item.tblForce.name).ToList();
                    break;
                case 6:
                    if (pageTableVariable.OrderType)
                        ViewTblFactors = ViewTblFactors.OrderBy(item => item.tblBranch.title).ToList();
                    else
                        ViewTblFactors = ViewTblFactors.OrderByDescending(item => item.tblBranch.title).ToList();
                    break;
                case 7:
                    if (pageTableVariable.OrderType)
                        ViewTblFactors = ViewTblFactors.OrderBy(item => item.status).ToList();
                    else
                        ViewTblFactors = ViewTblFactors.OrderByDescending(item => item.status).ToList();
                    break;
                default:
                    if (pageTableVariable.OrderType)
                        ViewTblFactors = ViewTblFactors.OrderBy(item => item.code).ToList();
                    else
                        ViewTblFactors = ViewTblFactors.OrderByDescending(item => item.code).ToList();
                    break;
            }

            return await Task.Run(() => ViewTblFactors);
        }

        public async Task<List<Factor.ViewTblFactor>> FindOrder(string FillterBy)
        {
            ViewTblFactors = (from item in Context.tblFactor
                              where item.code.Contains(FillterBy) &&
                                    item.status > 0
                              select new Factor.ViewTblFactor
                              {
                                  id = item.id,
                                  code = item.code,
                                  registerDate = item.registerDate,
                                  tblCustomer = item.tblCustomer,
                                  customerId = item.customerId,
                                  tblAssignToFactor = item.tblAssignToFactor,
                                  tblForce = item.tblForce,
                                  forceId = item.forceId,
                                  elctricShopId = item.elctricShopId,
                                  opratorId = item.opratorId,
                                  tblBranch = item.tblBranch,
                                  status = item.status
                              })
                         .OrderByDescending(item => item.code)
                         .ToList();

            return await Task.Run(() => ViewTblFactors);
        }

        public SaniDBEntities SaniDBEntities => Context as SaniDBEntities;
    }
}
