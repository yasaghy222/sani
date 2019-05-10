using DataLayer;
using ServiceLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ViewModel.General;

namespace Sani.Areas.Operator.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        private bool isExsist = false;
        private tblCustomer TblCustomer;

        [HttpGet]
        public ActionResult Index() => View();

        #region UserAjax
        // select all User item
        [HttpPost]
        public async Task<JsonResult> UsersList(Common.PageTableVariable pageTableVariable)
        {
            Resualt.Data = new
            {
                Records = await _unitOfWork.Customers.GetCustomerAsync(pageTableVariable),
                Total = await _unitOfWork.Customers.GetCountAsync()
            };
            return Resualt;
        }

        // search in Users list
        [HttpPost]
        public JsonResult SearchInUsers(string fillterBy)
        {
            Resualt.Data = _unitOfWork.Customers.FindCustomers(fillterBy);
            return Resualt;
        }

        //check user is exist
        [HttpPost]
        public JsonResult IsExist(string phoneNumber)
        {
            TblCustomer = _unitOfWork.Customers.Single(item => item.phoneNumber == phoneNumber);
            Resualt.Data = (TblCustomer != null) ? true : false;
            return Resualt;
        }

        // add User
        [HttpPost]
        public JsonResult Add(tblCustomer TblCustomer)
        {
            isExsist = _unitOfWork.Customers.SingleAny(item => item.phoneNumber == TblCustomer.phoneNumber);
            if (isExsist)
            {
                Resualt.Data = "این کاربر قبلا ثبت نام کرده است.";
            }
            else
            {
                TblCustomer.id = Utility.Functions.GenerateNewId();
                TblCustomer.mIntroduceCode = Utility.Functions.GenerateNewCode();
                TblCustomer.status = true;
                _unitOfWork.Customers.Add(TblCustomer);
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
            return Resualt;
        }

        // disable a user
        [HttpPost]
        public JsonResult DisableUser(Guid id)
        {
            TblCustomer = _unitOfWork.Customers.SingleById(id);
            TblCustomer.status = (TblCustomer.status) ? false : true;
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