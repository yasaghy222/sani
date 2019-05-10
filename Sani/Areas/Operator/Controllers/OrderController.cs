using AutoMapper;
using DataLayer;
using ServiceLayer.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Utility;
using ViewModel.Areas;
using ViewModel.General;

namespace Sani.Areas.Operator.Controllers
{
    public class OrderController : BaseController
    {
        public OrderController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        private int distance = 0;
        private bool isExist = false;
        private tblFactor Order = null;
        private string temp = string.Empty;
        private tblFactor TblFactor = null;
        private tblCustomer TblCustomer = null;
        private List<tblForce> TblForces = null;
        private Factor.FactorPreview preview = null;
        private List<tblService> TblServices = null;
        private tblCustomerAddress TblAddress = null;
        private List<tblCustomer> TblCustomers = null;
        private Customer.OrderAddress OrderAddress = null;
        private Services.OrderService OrderService = null;
        private List<tblAssignToFactor> newServices = null;
        private tblAssignToFactor TblAssignToFactor = null;
        private List<tblCustomerAddress> TblAddresses = null;
        private List<Services.OrderService> OrderServices = null;
        private List<Factor.ViewTblFactor> viewTblFactors = null;

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> AddEdit(Guid? id)
        {
            ViewBag.RFCount = await _unitOfWork.Force.GetCountAsync(item => item.status == 2);
            #region edit
            if (id != null)
            {
                Order = await _unitOfWork.Factor.SingleByIdAsync(id ?? Guid.Empty);
                ViewBag.Title = $"ویرایش سفارش {Order.code}";
            }
            #endregion
            #region add
            else
            {
                Order = new tblFactor();
                ViewBag.Title = "ثبت سفارش جدید";
            }
            #endregion
            return View(model: Order);
        }

        #region Order Ajax
        //select all orders
        public async Task<JsonResult> OrderList(Common.PageTableVariable pageTableVariable)
        {
            viewTblFactors = await _unitOfWork.Factor.GetOrdersAsync(pageTableVariable);
            Resualt.Data = new
            {
                Records = Mapper.Map<List<Factor.ViewOrder>>(viewTblFactors),
                Total = await _unitOfWork.Factor.GetCountAsync(item => item.status > 0)
            };
            return Resualt;
        }

        // search in order list
        [HttpPost]
        public JsonResult SearchInOrders(string fillterBy)
        {
            Resualt.Data = _unitOfWork.Factor.FindOrder(fillterBy);
            return Resualt;
        }

        //add edit order
        [HttpPost]
        public async Task<JsonResult> AddEditOrder(Factor.FactorVariable model)
        {
            #region variable
            long totalCount = 0;
            TblCustomer = await _unitOfWork.Customers.SingleByIdAsync(model.customerId);
            newServices = new List<tblAssignToFactor>();
            foreach (Services.OServiceVar item in model.services)
            {
                TblAssignToFactor = new tblAssignToFactor
                {
                    factorId = model.id,
                    serviceId = item.id,
                    count = item.count
                };
                totalCount += item.price * item.count;
                newServices.Add(TblAssignToFactor);
            };

            TblFactor = new tblFactor
            {
                id = model.id,
                customerId = model.customerId,
                forceId = model.forceId,
                bCode = model.bCode,
                opratorId = Guid.Parse(Request.Cookies["SSODesc"]["Id"]),
                isPrint = model.isPrint,
                description = model.description,
                addressId = model.addressId,
                totalCount = totalCount
            };
            #endregion

            #region edit mode
            if (TblFactor.id != Guid.Empty)
            {
                tblFactor oldOrder = await _unitOfWork.Factor.SingleByIdAsync(model.id);
                foreach (tblAssignToFactor i in newServices)
                {
                    i.factorId = TblFactor.id;
                    tblAssignToFactor oldService = oldOrder.tblAssignToFactor
                                                           .Single(item => item.factorId == i.factorId);
                    #region edit or delete order service
                    if (oldService != null)
                    {
                        #region update order service
                        if (oldService.count > i.count || oldService.count < i.count)
                        {
                            oldService.count = i.count;
                            await Task.Run(() => _unitOfWork.AssignToFactor.Update(oldService));
                        }
                        #endregion
                        #region delete order service
                        else if (i.count == 0)
                        {
                            await Task.Run(() => _unitOfWork.AssignToFactor.Remove(oldService));
                        }
                        #endregion
                    }
                    #endregion
                    #region add order service
                    else
                    {
                        await Task.Run(() => _unitOfWork.AssignToFactor.Add(i));
                    }
                    #endregion
                    try
                    {
                        await _unitOfWork.CompleteAsync();
                    }
                    catch (Exception e)
                    {
                        Resualt.Data = e.Message;
                    }
                }
                oldOrder.status = (byte)Common.FactorStatus.findForce;
                oldOrder.description = TblFactor.description;
                oldOrder.totalCount = totalCount;
                await _unitOfWork.CompleteAsync();
                try
                {
                    Resualt.Data = TblFactor.id;
                }
                catch (Exception)
                {
                    Resualt.Data = Guid.Empty;
                }
            }
            #endregion
            #region add mode
            else
            {
                TblFactor.id = Functions.GenerateNewId();
                TblFactor.code = $"SO-{Functions.GenerateNumCode() + DateTime.Now.Millisecond}";
                foreach (tblAssignToFactor item in newServices)
                {
                    item.factorId = TblFactor.id;
                }

                if (model.registerDate == null)
                {
                    TblFactor.registerDate = DateTime.Now;
                    TblFactor.status = (byte)Common.FactorStatus.findForce;
                }
                else
                {
                    TblFactor.registerDate = DateTime.Parse(model.registerDate);
                    TblFactor.status = (byte)Common.FactorStatus.reserve;
                }

                using (SaniDBEntities saniDB = new SaniDBEntities())
                {
                    using (DbContextTransaction dbTran = saniDB.Database.BeginTransaction())
                    {
                        try
                        {
                            await Task.Run(() => _unitOfWork.Factor.Add(TblFactor));
                            await _unitOfWork.CompleteAsync();

                            await Task.Run(() => _unitOfWork.AssignToFactor.AddRange(newServices));
                            await _unitOfWork.CompleteAsync();

                            dbTran.Commit();
                            Resualt.Data = TblFactor.id;
                        }
                        catch (Exception e)
                        {
                            dbTran.Rollback();
                            Resualt.Data = Guid.Empty;
                        }
                    }
                }
            }
            #endregion
            return Resualt;
        }

        //delete order
        [HttpPost]
        public async Task<JsonResult> DeleteOrder(Guid id)
        {
            TblFactor = await _unitOfWork.Factor.SingleByIdAsync(id);
            if (TblFactor != null)
            {
                using (SaniDBEntities saniDB = new SaniDBEntities())
                {
                    using (DbContextTransaction dbTran = saniDB.Database.BeginTransaction())
                    {
                        try
                        {
                            await Task.Run(() => _unitOfWork.AssignToFactor.RemoveRange(TblFactor.tblAssignToFactor));
                            await _unitOfWork.CompleteAsync();

                            await Task.Run(() => _unitOfWork.Factor.Remove(TblFactor));
                            await _unitOfWork.CompleteAsync();

                            dbTran.Commit();
                            Resualt.Data = "sucsess";
                        }
                        catch (Exception e)
                        {
                            dbTran.Rollback();
                            Resualt.Data = e.Message;
                        }
                    }
                }
            }
            else
            {
                Resualt.Data = "رکورد یافت نشد.";
            }
            return Resualt;
        }

        //find force
        [HttpPost]
        public async Task<JsonResult> FindingForce(Guid factorId, Guid? forceId)
        {
            TblFactor = await _unitOfWork.Factor.SingleByIdAsync(factorId);
            if (TblFactor != null)
            {
                #region find force
                if (forceId == null)
                {
                    TblFactor.tblAssignToFactor = newServices;
                    preview = Mapper.Map<Factor.FactorPreview>(TblFactor);
                    do
                    {
                        distance += 500;
                        TblForces = await FindForce(preview, distance);
                    } while (TblForces == null && distance < 5000);

                    if (TblForces.Count > 0 && distance <= 5000)
                    {
                        Resualt.Data = "found and send";
                    }
                    else
                    {
                        Resualt.Data = "not found";
                    }
                }
                #endregion
                #region send message to selected force
                else
                {
                    temp = await SendMessage(preview, forceId ?? Guid.Empty);
                    Resualt.Data = temp == "Success" ? "message is send" : "message is not send";
                }
                #endregion
            }
            else
            {
                Resualt.Data = "failed";
            }
            return Resualt;
        }
        #endregion

        #region Service
        //select all services for order
        [HttpPost]
        public JsonResult ServiceList(Guid? id, Guid bCode)
        {
            #region edit mode
            if (id != null)
            {
                int temp = 0;
                TblServices = _unitOfWork.Services.Find(item => item.status).ToList();
                OrderServices = new List<Services.OrderService>();
                foreach (tblService item in TblServices)
                {
                    IEnumerable<tblAssignToFactor> list = item.tblAssignToFactor.Where(x => x.factorId == id);
                    if (list.Count() > 0)
                    {
                        TblAssignToFactor = list.First();
                        temp = TblAssignToFactor.count;
                    }
                    isExist = item.tblBranchService.Any(x => x.bCode == bCode);
                    if (isExist)
                    {
                        OrderService = new Services.OrderService
                        {
                            id = item.id,
                            title = item.title,
                            count = temp,
                            price = item.tblBranchService.Single(x => x.bCode == bCode && x.serviceId == item.id).price,
                            catList = item.tblServiceCategory.GetCatList()
                        };
                        OrderServices.Add(OrderService);
                    }
                }
            }
            #endregion
            #region add mode
            else
            {
                TblServices = _unitOfWork.Services.Find(item => item.status).ToList();
                OrderServices = new List<Services.OrderService>();
                foreach (tblService item in TblServices)
                {
                    isExist = item.tblBranchService.Any(x => x.bCode == bCode);
                    if (isExist)
                    {
                        OrderService = new Services.OrderService
                        {
                            id = item.id,
                            title = item.title,
                            count = 0,
                            price = item.tblBranchService.Single(x => x.bCode == bCode).price,
                            catList = item.tblServiceCategory.GetCatList()
                        };
                        OrderServices.Add(OrderService);
                    }
                }
            }
            #endregion
            Resualt.Data = OrderServices;
            return Resualt;
        }

        // search in service list
        [HttpPost]
        public JsonResult SearchInService(string fillterBy, Guid? id)
        {
            #region edit mode
            if (id != null)
            {
                int temp = 0;
                TblServices = _unitOfWork.Services.Find(item => item.status &&
                                                                item.title.Contains(fillterBy))
                                                                .ToList();
                OrderServices = new List<Services.OrderService>();
                foreach (tblService item in TblServices)
                {
                    if (item.tblAssignToFactor.Count > 0)
                    {
                        TblAssignToFactor = item.tblAssignToFactor.Single(x => x.factorId == id);
                        temp = TblAssignToFactor.count;
                    }
                    OrderService = new Services.OrderService
                    {
                        id = item.id,
                        title = item.title,
                        count = temp,
                        catList = item.tblServiceCategory.GetCatList()
                    };
                    OrderServices.Add(OrderService);
                }
                Resualt.Data = OrderServices;
            }
            #endregion
            #region add mode
            else
            {
                TblServices = _unitOfWork.Services.Find(item => item.status &&
                                                                item.title.Contains(fillterBy))
                                                                .ToList();
                Resualt.Data = Mapper.Map<List<Services.OrderService>>(TblServices);
            }
            #endregion
            return Resualt;
        }
        #endregion

        #region Customer
        //select all services for order
        [HttpPost]
        public JsonResult CustomerList(Guid? id)
        {
            #region edit mode
            if (id != null)
            {
                TblCustomer = _unitOfWork.Factor.SingleById(id ?? Guid.Empty).tblCustomer;
                TblCustomers = new List<tblCustomer>
                {
                    TblCustomer
                };
            }
            #endregion
            #region add mode
            else
            {
                TblCustomers = _unitOfWork.Customers.Find(item => item.status).Take(10).ToList();
            }
            #endregion
            Resualt.Data = Mapper.Map<List<Customer.OrderCustomer>>(TblCustomers);
            return Resualt;
        }

        // search in service list
        [HttpPost]
        public JsonResult SearchInCustomer(string fillterBy)
        {
            TblCustomers = _unitOfWork.Customers.Find(item => item.status &&
                                                              item.phoneNumber.Contains(fillterBy))
                                                .ToList();
            Resualt.Data = Mapper.Map<List<Customer.OrderCustomer>>(TblCustomers);
            return Resualt;
        }
        #endregion

        #region Address
        //select all services for order
        [HttpPost]
        public async Task<JsonResult> AddressList(Guid? factId, Guid customerId)
        {
            #region edit mode
            if (factId != null)
            {
                bool temp = false;
                TblAddress = await Task.Run(() => _unitOfWork.Factor.SingleById(factId ?? Guid.Empty).tblCustomerAddress);
                TblAddresses = await Task.Run(() => _unitOfWork.CustomerAddress.Find(item => item.customerId == customerId).ToList());
                List<Customer.OrderAddress> OrderAddresses = new List<Customer.OrderAddress>();
                foreach (tblCustomerAddress item in TblAddresses)
                {
                    temp = item == TblAddress ? true : false;
                    OrderAddress = new Customer.OrderAddress
                    {
                        id = item.id,
                        locationId = item.locationId,
                        cityName = item.tblState_City.title,
                        address = item.address,
                        lat = item.lat,
                        lng = item.lng,
                        isSelected = temp
                    };
                    OrderAddresses.Add(OrderAddress);
                }
                Resualt.Data = OrderAddresses;
            }
            #endregion
            #region add mode
            else
            {
                TblAddresses = _unitOfWork.CustomerAddress.Find(item => item.customerId == customerId).ToList();
                Resualt.Data = Mapper.Map<List<Customer.OrderAddress>>(TblAddresses);
            }
            #endregion
            return Resualt;
        }

        //add customer address
        [HttpPost]
        public JsonResult AddAddress(Customer.OAddressVar model)
        {
            model.lat = model.lat.Replace('.', '/');
            model.lng = model.lng.Replace('.', '/');
            tblCustomerAddress address = Mapper.Map<tblCustomerAddress>(model);
            _unitOfWork.CustomerAddress.Add(address);
            try
            {
                _unitOfWork.Complete();
                Resualt.Data = "عملیات با موفقیت انجام شد.";
            }
            catch (Exception e)
            {
                Resualt.Data = e.Message;
            }
            return Resualt;
        }

        //delete customer address
        [HttpPost]
        public JsonResult DelAddress(Guid id)
        {
            TblAddress = _unitOfWork.CustomerAddress.SingleById(id);
            _unitOfWork.CustomerAddress.Remove(TblAddress);
            try
            {
                _unitOfWork.Complete();
                Resualt.Data = "عملیات با موفقیت انجام شد.";
            }
            catch (Exception e)
            {
                Resualt.Data = e.Message;
            }
            return Resualt;
        }
        #endregion
    }
}