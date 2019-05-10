using DataLayer;
using ServiceLayer.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Core.Repositories
{
    public class PaymentLogsRepository : GenericRepository<tblPaymentLogs>, IPaymentLogsRepository
    {
        public PaymentLogsRepository(SaniDBEntities context) : base(context) { }
        public SaniDBEntities SaniDBEntities => Context as SaniDBEntities;
    }
}
