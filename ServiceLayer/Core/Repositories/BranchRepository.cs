using DataLayer;
using ServiceLayer.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Areas;

namespace ServiceLayer.Core.Repositories
{
    public class BranchRepository : GenericRepository<tblBranch>, IBranchRepository
    {
        public BranchRepository(SaniDBEntities context) : base(context) { }

        public List<Branches.SelectBranch> SelectBranches()
        {
            return (from item in Context.tblBranch
                    orderby item.title
                    select new Branches.SelectBranch
                    {
                        Text = item.title,
                        Value = item.id
                    })
                    .ToList();
        }

        public SaniDBEntities SaniDBEntities => Context as SaniDBEntities;
    }
}
