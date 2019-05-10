using AutoMapper;
using DataLayer;
using Newtonsoft.Json;
using ServiceLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Utility;
using ViewModel.Areas;
using ViewModel.General;
using static ViewModel.Areas.Services;

namespace Sani.Areas.Operator.Controllers
{
    public class ServiceController : BaseController
    {

        public ServiceController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        private bool isExsist = false;
        private tblService TblService;
        private tblBranchService TblBranchService;
        private tblServiceCategory TblServiceCategory;

        [HttpGet]
        public ActionResult Index() => View();

        [HttpGet]
        public ActionResult AddEdit(Guid? id)
        {
            #region edit
            if (id != null)
            {
                TblService = _unitOfWork.Services.SingleById(id ?? Guid.Empty);
                ViewBag.Title = $"ویرایش تعرفه {TblService.title}";
            }
            #endregion
            #region add
            else
            {
                TblService = new tblService();
                ViewBag.Title = "ثبت تعرفه جدید";
            }
            #endregion
            return View(model: TblService);
        }

        [HttpGet]
        public ActionResult Category() => View();

        #region ServiceAjax
        // select all Service item
        [HttpPost]
        public async Task<JsonResult> ServiceList(Common.PageTableVariable pageTableVariable)
        {
            Resualt.Data = new
            {
                Records = await _unitOfWork.Services.GetServicesAsync(pageTableVariable),
                Total = await _unitOfWork.Services.GetCountAsync()
            };
            return Resualt;
        }

        // search in Servicelist
        [HttpPost]
        public JsonResult SearchInServices(string fillterBy)
        {
            Resualt.Data = _unitOfWork.Services.FindServices(fillterBy);
            return Resualt;
        }

        // add or edit service
        [HttpPost]
        public JsonResult AddEdit(tblService model)
        {
            #region edit
            if (model.id != Guid.Empty)
            {
                _unitOfWork.Services.Update(model);
                try
                {
                    _unitOfWork.Complete();
                    try
                    {
                        Resualt.Data = new
                        {
                            resualt = "عملیات با موفقیت انجام شد.",
                            id = (model.id).ToString()
                        };
                    }
                    catch (Exception e)
                    {
                        Resualt.Data = e.Message;
                    }
                }
                catch (Exception e)
                {
                    Resualt.Data = e.Message;
                }
            }
            #endregion
            #region add
            else
            {
                isExsist = _unitOfWork.Services
                                      .SingleAny(item => item.title == model.title && 
                                                         item.catId == item.catId);
                if (isExsist)
                {
                    Resualt.Data = "این عنوان قبلا در این دسته ثبت شده است.";
                }
                else
                {
                    model.id = Functions.GenerateNewId();
                    _unitOfWork.Services.Add(model);
                    try
                    {
                        _unitOfWork.Complete();
                        Resualt.Data = new
                        {
                            resualt = "عملیات با موفقیت انجام شد.",
                            id = (model.id).ToString()
                        };
                    }
                    catch (Exception e)
                    {
                        Resualt.Data = $"{e.Message}";
                    }
                }
            }
            #endregion
            return Resualt;
        }

        // delete a service
        [HttpPost]
        public JsonResult DeleteService(Guid id)
        {
            TblService = _unitOfWork.Services.SingleById(id);
            _unitOfWork.Services.Remove(TblService);
            try
            {
                _unitOfWork.Complete();
                Resualt.Data = "عملیات با موفقیت انجام شد.";
            }
            catch (Exception e)
            {
                Resualt.Data = $"{e.Message}";
            }

            return Resualt;
        }

        #endregion

        #region ServiceAssign
        [HttpPost]
        public JsonResult ServiceAssignList(Guid id)
        {
            Resualt.Data = _unitOfWork.ServiceAssign.GetServiceAssign(id);
            return Resualt;
        }


        // add or edit service
        [HttpPost]
        public JsonResult AddEditServiceAssign(AssignService.ViewTblBranchService viewTblBranchService)
        {
            TblBranchService = new tblBranchService
            {
                bCode = viewTblBranchService.bCode,
                serviceId = viewTblBranchService.serviceId,
                price = viewTblBranchService.price
            };

            #region edit
            if (viewTblBranchService.formMode)
            {
                tblBranchService old = _unitOfWork.ServiceAssign
                                       .Single(item =>
                                                    item.bCode == TblBranchService.bCode &&
                                                    item.serviceId == TblBranchService.serviceId);
                old.price = TblBranchService.price;
                try
                {
                    _unitOfWork.Complete();
                    Resualt.Data = "عملیات با موفقیت انجام شد.";
                }
                catch (Exception e)
                {
                    Resualt.Data = $"{e.Message}";
                }
            }
            #endregion
            #region add
            else
            {
                isExsist = _unitOfWork.ServiceAssign.SingleAny(item =>
                                                                    item.serviceId == TblBranchService.serviceId &&
                                                                    item.bCode == TblBranchService.bCode
                                                            );
                if (isExsist)
                {
                    Resualt.Data = $" اختصاص به این نمایندگی قبلا ثبت شده است. ";
                }
                else
                {
                    _unitOfWork.ServiceAssign.Add(TblBranchService);
                    try
                    {
                        _unitOfWork.Complete();
                        Resualt.Data = "عملیات با موفقیت انجام شد.";
                    }
                    catch (Exception e)
                    {
                        Resualt.Data = $"{e.Message}";
                    }
                }
            }
            #endregion
            return Resualt;
        }

        // delete a service
        [HttpPost]
        public JsonResult DeleteServiceAssign(Guid bCode, Guid serviceId)
        {
            TblBranchService = _unitOfWork.ServiceAssign.Single(item => item.bCode == bCode && item.serviceId == serviceId);
            _unitOfWork.ServiceAssign.Remove(TblBranchService);
            try
            {
                _unitOfWork.Complete();
                Resualt.Data = "عملیات با موفقیت انجام شد.";
            }
            catch (Exception e)
            {
                Resualt.Data = $"{e.Message}";
            }

            return Resualt;
        }
        #endregion

        #region ServiceCategory
        [HttpPost]
        public JsonResult ServiceCatList()
        {
            Resualt.Data = _unitOfWork.ServiceCategoryes.GetServicesCatList();
            return Resualt;
        }

        // add or edit service category
        [HttpPost]
        public JsonResult AddEditServiceCat(tblServiceCategory TblServiceCategory)
        {
            #region edit
            if (TblServiceCategory.id != Guid.Empty)
            {
                tblServiceCategory old = _unitOfWork.ServiceCategoryes.Single(item => item.id == TblServiceCategory.id);
                old.title = TblServiceCategory.title;
                try
                {
                    _unitOfWork.Complete();
                    Resualt.Data = "عملیات با موفقیت انجام شد.";
                }
                catch (Exception e)
                {
                    Resualt.Data = $"{e.Message}";
                }
            }
            #endregion
            #region add
            else
            {
                if (TblServiceCategory.pid != Guid.Empty)
                {
                    isExsist = _unitOfWork.ServiceCategoryes.SingleAny(item =>
                                                                            item.title == TblServiceCategory.title &&
                                                                            item.pid == TblServiceCategory.pid);
                }
                else
                {
                    isExsist = _unitOfWork.ServiceCategoryes.SingleAny(item => item.title == TblServiceCategory.title);
                }
                if (isExsist)
                {
                    Resualt.Data = $" این دسته قبلا ثبت شده است. ";
                }
                else
                {
                    TblServiceCategory.id = Functions.GenerateNewId();
                    if (TblServiceCategory.pid == Guid.Empty)
                    {
                        TblServiceCategory.pid = TblServiceCategory.id;
                    }
                    _unitOfWork.ServiceCategoryes.Add(TblServiceCategory);
                    try
                    {
                        _unitOfWork.Complete();
                        Resualt.Data = "عملیات با موفقیت انجام شد.";
                    }
                    catch (Exception e)
                    {
                        Resualt.Data = $"{e.Message}";
                    }
                }
            }
            #endregion
            return Resualt;
        }

        // delete a service category
        [HttpPost]
        public JsonResult DeleteServiceCat(Guid id)
        {
            TblServiceCategory = _unitOfWork.ServiceCategoryes.Single(item => item.id == id);
            _unitOfWork.ServiceCategoryes.Remove(TblServiceCategory);
            try
            {
                _unitOfWork.Complete();
                Resualt.Data = "عملیات با موفقیت انجام شد.";
            }
            catch (Exception e)
            {
                Resualt.Data = $"{e.Message}";
            }

            return Resualt;
        }
        #endregion
    }
}