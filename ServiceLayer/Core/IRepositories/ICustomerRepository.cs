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
    public interface ICustomerRepository : IGenericRepository<tblCustomer>
    {
        Task<List<Customer.ViewCustomerList>> GetCustomerAsync(Common.PageTableVariable pageTableVariable);
        List<Customer.ViewCustomerList> FindCustomers(string FillterBy);
    }
}
