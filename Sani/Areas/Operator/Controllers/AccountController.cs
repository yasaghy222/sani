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

namespace Sani.Areas.Operator.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        private bool tempBool = false;
        private tblOprator Oprator = null;
        private string temp = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ForgetPass()
        {
            return View();
        }

        #region account authorize ajax
        private void SetCookie(tblOprator oprator)
        {
            HttpCookie cookie = new HttpCookie("SSODesc")
            {
                Expires = DateTime.Now.AddMonths(6)
            };
            cookie.Values.Add("Id", oprator.id.ToString());
            cookie.Values.Add("Token", oprator.token);
            cookie.Values.Add("Name", oprator.name);
            cookie.Values.Add("Role", oprator.tblRole.description);
            HttpContext.Response.SetCookie(cookie);
        }

        private async Task<bool> IsValid(tblOprator oprator, Account.LoginVar loginVar)
        {
            temp = Functions.GenerateHash(loginVar.passWord);
            if (oprator.pass == temp &&
                oprator.loginFailedCount < 5 &&
                oprator.status)
            {
                oprator.loginFailedCount = 0;
                oprator.token = Functions.GenerateNewToken();
                await _unitOfWork.CompleteAsync();
                try
                {
                    SetCookie(oprator);
                    tempBool = true;
                }
                catch { }
            }
            return tempBool;
        }

        //login to account
        [HttpPost]
        public async Task<JsonResult> Login(Account.LoginVar loginVar)
        {
            if (loginVar.phoneNumber != null && loginVar.passWord != null)
            {
                Oprator = await _unitOfWork.Oprator
                                           .SingleAsync(item =>
                                                        item.phoneNumber == loginVar.phoneNumber);
                if (Oprator != null)
                {
                    if (await IsValid(Oprator, loginVar))
                    {
                        Resualt.Data = "success";
                    }
                    else
                    {
                        Resualt.Data = "شماره موبایل یا رمز عبور اشتباه است.";
                    }
                }
                else
                {
                    Resualt.Data = "اپراتوری با مشخصات وارد شده یافت نشد.";
                }
            }
            else
            {
                Resualt.Data = "لطفا شماره تلفن و رمز عبور را وارد کنید.";
            }
            return Resualt;
        }

        //logout from account
        [HttpPost]
        public async Task<JsonResult> Logout(string token)
        {
            Oprator = await _unitOfWork.Oprator.SingleAsync(item => item.token == token);
            if (Oprator != null)
            {
                Oprator.token = "";
                await _unitOfWork.CompleteAsync();
                try
                {
                    Request.Cookies.Remove("Token");
                    Resualt.Data = "success";
                }
                catch (Exception e)
                {
                    Resualt.Data = e.Message;
                }
            }
            else
            {
                Resualt.Data = "اپراتور یافت نشد.";
            }
            return Resualt;
        }

        //forget password 
        [HttpPost]
        public async Task<JsonResult> ForgetPass(Account.ForgetPassVar forgetPassVar)
        {
            return Resualt;
        }
        #endregion
    }
}