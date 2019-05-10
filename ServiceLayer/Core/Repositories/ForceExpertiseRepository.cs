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
    public class ForceExpertiseRepository : GenericRepository<tblForceExpertise>, IForceExpertiseRepository
    {
        public ForceExpertiseRepository(SaniDBEntities context) : base(context) { }

        List<Force.ViewForceExp> expList = null;

        public List<Force.ViewForceExp> ExpList(Guid id)
        {
            expList = (from item in Context.tblForceExpertise
                       orderby item.tblServiceCategory.title
                       where item.forceId == id
                       select new Force.ViewForceExp
                       {
                           catId = item.catId,
                           forceId = item.forceId,
                           expTitle = item.tblServiceCategory.title,
                           cent = item.cent
                       })
                       .ToList();
            return expList;
        }

        public SaniDBEntities SaniDBEntities => Context as SaniDBEntities;
    }
}
