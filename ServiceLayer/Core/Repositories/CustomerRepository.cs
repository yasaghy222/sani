using DataLayer;
using ServiceLayer.Core.IRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Areas;
using ViewModel.General;

namespace ServiceLayer.Core.Repositories
{
    public class CustomerRepository : GenericRepository<tblCustomer>, ICustomerRepository
    {
        public CustomerRepository(SaniDBEntities context) : base(context) { }
        private List<Customer.ViewCustomerList> ViewUsersLists;
        public async Task<List<Customer.ViewCustomerList>> GetCustomerAsync(Common.PageTableVariable pageTableVariable)
        {
            ViewUsersLists = (from item in Context.tblCustomer
                              select new Customer.ViewCustomerList
                              {
                                  id = item.id,
                                  name = item.name,
                                  phoneNumber = item.phoneNumber,
                                  creditor = item.creditor,
                                  cityName = item.tblState_City.title,
                                  status = item.status
                              })
                                .OrderByDescending(item => item.name)
                                .Skip((pageTableVariable.PageIndex - 1) * pageTableVariable.PageSize)
                                .Take(pageTableVariable.PageSize)
                                .ToList();

            switch (pageTableVariable.OrderBy)
            {
                case 0:
                    if (pageTableVariable.OrderType)
                        ViewUsersLists = ViewUsersLists.OrderBy(item => item.name).ToList();
                    else
                        ViewUsersLists = ViewUsersLists.OrderByDescending(item => item.name).ToList();
                    break;
                case 1:
                    if (pageTableVariable.OrderType)
                        ViewUsersLists = ViewUsersLists.OrderBy(item => item.phoneNumber).ToList();
                    else
                        ViewUsersLists = ViewUsersLists.OrderByDescending(item => item.phoneNumber).ToList();
                    break;
                case 2:
                    if (pageTableVariable.OrderType)
                        ViewUsersLists = ViewUsersLists.OrderBy(item => item.creditor).ToList();
                    else
                        ViewUsersLists = ViewUsersLists.OrderByDescending(item => item.creditor).ToList();
                    break;
                case 3:
                    if (pageTableVariable.OrderType)
                        ViewUsersLists = ViewUsersLists.OrderBy(item => item.cityName).ToList();
                    else
                        ViewUsersLists = ViewUsersLists.OrderByDescending(item => item.cityName).ToList();
                    break;
                case 4:
                    if (pageTableVariable.OrderType)
                        ViewUsersLists = ViewUsersLists.OrderBy(item => item.status).ToList();
                    else
                        ViewUsersLists = ViewUsersLists.OrderByDescending(item => item.status).ToList();
                    break;
                default:
                    if (pageTableVariable.OrderType)
                        ViewUsersLists= ViewUsersLists.OrderBy(item => item.name).ToList();
                    else
                        ViewUsersLists = ViewUsersLists.OrderByDescending(item => item.name).ToList();
                    break;
            }

            return await Task.Run(() => ViewUsersLists);
        }
        public List<Customer.ViewCustomerList> FindCustomers(string FillterBy)
        {
            ViewUsersLists = (from item in Context.tblCustomer
                                where item.phoneNumber.Contains(FillterBy)
                                select new Customer.ViewCustomerList
                                {
                                    id = item.id,
                                    name = item.name,
                                    phoneNumber = item.phoneNumber,
                                    creditor = item.creditor,
                                    cityName = item.tblState_City.title,
                                    status = item.status
                                })
                                .OrderByDescending(item => item.name)
                                .ToList();

            return ViewUsersLists;
        }
        public SaniDBEntities SaniDBEntities => Context as SaniDBEntities;
    }
}
