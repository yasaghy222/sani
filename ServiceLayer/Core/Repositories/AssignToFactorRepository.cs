using DataLayer;
using ServiceLayer.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Core.Repositories
{
    public class AssignToFactorRepository:GenericRepository<tblAssignToFactor>,IAssignToFactorRepository
    {
        public AssignToFactorRepository(SaniDBEntities context) : base(context) { }
        public SaniDBEntities SaniDBEntities => Context as SaniDBEntities;
    }
}
