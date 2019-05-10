﻿using DataLayer;
using ServiceLayer.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Core.Repositories
{
    public class ForceCancelsRepository : GenericRepository<tblForceCancels> , IForceCancelsRepository
    {
        public ForceCancelsRepository(SaniDBEntities context) : base(context) { }
        public SaniDBEntities SaniDBEntities => Context as SaniDBEntities;
    }
}