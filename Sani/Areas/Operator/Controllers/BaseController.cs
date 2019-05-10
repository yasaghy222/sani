using AutoMapper;
using DataLayer;
using ServiceLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Utility;
using ViewModel.Areas;
using ViewModel.General;

namespace Sani.Areas.Operator.Controllers
{
    public class BaseController : Controller
    {
        private bool tempBool = false;
        private double tempDistance = 0;
        private string temp = string.Empty;
        private List<tblForce> TblForces = null;
        protected JsonResult Resualt = new JsonResult();

        protected readonly IUnitOfWork _unitOfWork;
        public BaseController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        #region general select item
        [HttpPost]
        public JsonResult ServiceCatSelect()
        {
            Resualt.Data = _unitOfWork.ServiceCategoryes.SelectServiceCat();
            return Resualt;
        }

        [HttpPost]
        public JsonResult BranchSelect()
        {
            Resualt.Data = _unitOfWork.Branches.SelectBranches();
            return Resualt;
        }

        [HttpPost]
        public JsonResult CitySelect()
        {
            Resualt.Data = _unitOfWork.StateCity.SelectCity();
            return Resualt;
        }

        [HttpPost]
        public JsonResult AForceSelect()
        {
            Resualt.Data = _unitOfWork.Force.SelectAForce();
            return Resualt;
        }

        [HttpPost]
        public async Task<JsonResult> TopOrders()
        {
            Common.PageTableVariable pVar = new Common.PageTableVariable
            {
                OrderBy = 0,
                OrderType = false,
                PageIndex = 1,
                PageSize = 5
            };

            List<Factor.ViewTblFactor> viewTblFactors = await _unitOfWork.Factor.GetOrdersAsync(pVar);
            Resualt.Data = Mapper.Map<List<Factor.ViewOrder>>(viewTblFactors);
            return Resualt;
        }

        /// <summary>
        /// یافتن نیرو برای یک سفارش
        /// </summary>
        /// <param name="factorFindForceVariable"></param>
        [HttpPost]
        protected async Task<List<tblForce>> FindForce(Factor.FactorPreview preview, double distance)
        {
            List<tblForce> tempList = new List<tblForce>();
            TblForces = await Task.Run(() =>
                                             _unitOfWork.Force.Find(item =>
                                                                            item.status == (byte)Common.ForceStatus.ready &&
                                                                            item.rate > 4 &&
                                                                            item.debtor < 50000)
                                                               .Take(5)
                                                               .OrderByDescending(item => item.rate)
                                                               .ToList());

            foreach (var item in TblForces)
            {
                tempDistance = Functions.CalcDistance(preview.lat,
                                                      preview.lng,
                                                      item.lat,
                                                      item.lng);
                if (tempDistance < distance)
                {
                    tempList.Add(item);
                    temp = await ServiceLayer.Pushe.Pushe.SendNotifiction(item.pusheId, preview);
                }
            }
            if (temp == null)
            {
                tempList = null;
            }
            return tempList;
        }

        [HttpPost]
        protected async Task<string> SendMessage(Factor.FactorPreview preview, Guid forceId)
        {
            temp = await Task.Run(() => _unitOfWork.Force.SingleById(forceId).pusheId);
            temp = await ServiceLayer.Pushe.Pushe.SendNotifiction(temp, preview);
            return temp;
        }
        #endregion

        #region authorize and authentication
        public bool CheckLogin()
        {
            if (Request.Browser.Cookies && Request.Cookies.Count > 0)
            {
                temp = Request.Cookies["SSODesc"]["Token"];
                tempBool = (temp != null) ? true : false;
                if (tempBool)
                {
                    tempBool = _unitOfWork.Oprator.SingleAny(item => item.token == temp);
                }
            }
            return tempBool;
        }

        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            string Controller = context.RouteData.Values["Controller"].ToString(),
                   Action = context.RouteData.Values["Action"].ToString();

            if (Controller != "Account")
            {
                if (!CheckLogin())
                {
                    tempBool = context.HttpContext.Request.IsAjaxRequest();
                    ViewBag.Message = "توکن شما منقضی شده است و یا از مرورگر دیگر وارد سیستم شده‌اید.";

                    if (tempBool)
                    {
                        context.Result = new JsonResult
                        {
                            Data = new { Redirect = "/SS-OManage/Login" },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        context.Result = new RedirectResult("~/SS-OManage/Login");
                    }
                }
            }
            else if (Controller == "Account" && Action != "Logout" && CheckLogin())
            {
                context.Result = new RedirectResult("~/SS-OManage");
            }
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}