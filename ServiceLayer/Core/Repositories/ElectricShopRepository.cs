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
    public class ElectricShopRepository : GenericRepository<tblElctricShop> , IElectricShopRepository
    {
        public ElectricShopRepository(SaniDBEntities context) : base(context) { }

        private List<EShop.ViewTblEShop> TblEShopList;
        public async Task<List<EShop.ViewTblEShop>> GetEShopAsync(Common.PageTableVariable pageTableVariable)
        {
            TblEShopList = (from item in Context.tblElctricShop
                            select new EShop.ViewTblEShop
                            {
                                id = item.id,
                                title = item.title,
                                managerName = item.managerName,
                                managerPhoneNumber = item.managerPhoneNumber,
                                creditor = item.creditor,
                                bCode = item.bCode,
                                status = item.status,
                                tblBranch = item.tblBranch
                            })
                                .OrderByDescending(item => item.managerName)
                                .Skip((pageTableVariable.PageIndex - 1) * pageTableVariable.PageSize)
                                .Take(pageTableVariable.PageSize)
                                .ToList();


            switch (pageTableVariable.OrderBy)
            {
                case 0:
                    if (pageTableVariable.OrderType)
                        TblEShopList = TblEShopList.OrderBy(item => item.title).ToList();
                    else
                        TblEShopList = TblEShopList.OrderByDescending(item => item.title).ToList();
                    break;
                case 1:
                    if (pageTableVariable.OrderType)
                        TblEShopList = TblEShopList.OrderBy(item => item.managerName).ToList();
                    else
                        TblEShopList = TblEShopList.OrderByDescending(item => item.managerName).ToList();
                    break;
                case 2:
                    if (pageTableVariable.OrderType)
                        TblEShopList = TblEShopList.OrderBy(item => item.managerPhoneNumber).ToList();
                    else
                        TblEShopList = TblEShopList.OrderByDescending(item => item.managerPhoneNumber).ToList();
                    break;
                case 3:
                    if (pageTableVariable.OrderType)
                        TblEShopList = TblEShopList.OrderBy(item => item.creditor).ToList();
                    else
                        TblEShopList = TblEShopList.OrderByDescending(item => item.creditor).ToList();
                    break;
                case 4:
                    if (pageTableVariable.OrderType)
                        TblEShopList = TblEShopList.OrderBy(item => item.bCode).ToList();
                    else
                        TblEShopList = TblEShopList.OrderByDescending(item => item.bCode).ToList();
                    break;
                case 5:
                    if (pageTableVariable.OrderType)
                        TblEShopList = TblEShopList.OrderBy(item => item.status).ToList();
                    else
                        TblEShopList = TblEShopList.OrderByDescending(item => item.status).ToList();
                    break;
                default:
                    if (pageTableVariable.OrderType)
                        TblEShopList = TblEShopList.OrderBy(item => item.managerName).ToList();
                    else
                        TblEShopList = TblEShopList.OrderByDescending(item => item.managerName).ToList();
                    break;
            }

            return await Task.Run(() => TblEShopList);
        }
        public List<EShop.ViewTblEShop> FindEShop(string FillterBy)
        {
            TblEShopList = (from item in Context.tblElctricShop
                            where item.managerName.Contains(FillterBy)
                            select new EShop.ViewTblEShop
                            {
                                id = item.id,
                                title = item.title,
                                managerName = item.managerName,
                                managerPhoneNumber = item.managerPhoneNumber,
                                creditor = item.creditor,
                                bCode = item.bCode,
                                status = item.status,
                                tblBranch = item.tblBranch
                            })
                                .OrderByDescending(item => item.managerName)
                                .ToList();

            return TblEShopList;
        }

        public SaniDBEntities SaniDBEntities => Context as SaniDBEntities;
    }
}
