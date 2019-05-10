using DataLayer;
using ServiceLayer.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using ViewModel.Areas;
using ViewModel.General;

namespace ServiceLayer.Core.Repositories
{
    public class ForceRepository : GenericRepository<tblForce>, IForceRepository
    {
        public ForceRepository(SaniDBEntities context) : base(context) { }

        private List<Force.ViewTblForce> TblForceList;
        public async Task<List<Force.ViewTblForce>> GetForceAsync(Common.PageTableVariable pageTableVariable)
        {
            TblForceList = (from item in Context.tblForce
                            select new Force.ViewTblForce
                            {
                                id = item.id,
                                name = item.name,
                                phoneNumber = item.phoneNumber,
                                creditor = item.creditor,
                                debtor = item.debtor,
                                bCode = item.bCode,
                                status = item.status,
                                tblBranch = item.tblBranch
                            })
                                .OrderByDescending(item => item.name)
                                .Skip((pageTableVariable.PageIndex - 1) * pageTableVariable.PageSize)
                                .Take(pageTableVariable.PageSize)
                                .ToList();


            switch (pageTableVariable.OrderBy)
            {
                case 0:
                    if (pageTableVariable.OrderType)
                        TblForceList = TblForceList.OrderBy(item => item.name).ToList();
                    else
                        TblForceList = TblForceList.OrderByDescending(item => item.name).ToList();
                    break;
                case 1:
                    if (pageTableVariable.OrderType)
                        TblForceList = TblForceList.OrderBy(item => item.phoneNumber).ToList();
                    else
                        TblForceList = TblForceList.OrderByDescending(item => item.phoneNumber).ToList();
                    break;
                case 2:
                    if (pageTableVariable.OrderType)
                        TblForceList = TblForceList.OrderBy(item => item.creditor).ToList();
                    else
                        TblForceList = TblForceList.OrderByDescending(item => item.creditor).ToList();
                    break;
                case 3:
                    if (pageTableVariable.OrderType)
                        TblForceList = TblForceList.OrderBy(item => item.debtor).ToList();
                    else
                        TblForceList = TblForceList.OrderByDescending(item => item.debtor).ToList();
                    break;
                case 4:
                    if (pageTableVariable.OrderType)
                        TblForceList = TblForceList.OrderBy(item => item.bCode).ToList();
                    else
                        TblForceList = TblForceList.OrderByDescending(item => item.bCode).ToList();
                    break;
                case 5:
                    if (pageTableVariable.OrderType)
                        TblForceList = TblForceList.OrderBy(item => item.status).ToList();
                    else
                        TblForceList = TblForceList.OrderByDescending(item => item.status).ToList();
                    break;
                default:
                    if (pageTableVariable.OrderType)
                        TblForceList = TblForceList.OrderBy(item => item.name).ToList();
                    else
                        TblForceList = TblForceList.OrderByDescending(item => item.name).ToList();
                    break;
            }

            return await Task.Run(() => TblForceList);
        }
        public List<Force.ViewTblForce> FindForce(string FillterBy)
        {
            TblForceList = (from item in Context.tblForce
                            where item.name.Contains(FillterBy)
                            select new Force.ViewTblForce
                            {
                                id = item.id,
                                name = item.name,
                                phoneNumber = item.phoneNumber,
                                creditor = item.creditor,
                                debtor = item.debtor,
                                bCode = item.bCode,
                                status = item.status,
                                tblBranch = item.tblBranch
                            })
                                .OrderByDescending(item => item.name)
                                .ToList();

            return TblForceList;
        }

        public List<Force.SelectAForce> SelectAForce()
        {
            byte temp = (byte) Common.ForceStatus.ready;
            return (from item in Context.tblForce
                    where item.status == temp
                    orderby item.rate
                    select new Force.SelectAForce
                    {
                        Text = item.name,
                        Value = item.id
                    })
                    .ToList();
        }

        public SaniDBEntities SaniDBEntities => Context as SaniDBEntities;
    }
}
