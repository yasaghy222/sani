﻿using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Areas;

namespace ServiceLayer.Core.IRepositories
{
    public interface IBranchRepository : IGenericRepository<tblBranch>
    {
        List<Branches.SelectBranch> SelectBranches();
    }
}