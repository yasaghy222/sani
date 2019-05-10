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
    public class EShopController : BaseController
    {

        public EShopController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        private bool isExist = false;
        private string temp = string.Empty;
        private tblElctricShop TblEShop = null;
        private tblDocuments TblDocuments = null;
        private List<tblDocuments> TblDocList = null;
        private List<Common.ViewTblDoc> ViewDocList = null;
        private List<EShop.ViewTblEShop> TblEShopList = null;

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddEdit(Guid? id)
        {
            #region edit
            if (id != null)
            {
                TblEShop = _unitOfWork.ElectricShops.SingleById(id ?? Guid.Empty);
                ViewBag.Title = $"ویرایش الکتریکی {TblEShop.title}";
            }
            #endregion
            #region add
            else
            {
                TblEShop = new tblElctricShop();
                ViewBag.Title = "ثبت الکتریکی جدید";
            }
            #endregion
            return View(model: TblEShop);
        }

        #region EShopAjax
        // select all electric shop
        [HttpPost]
        public async Task<JsonResult> EShopList(Common.PageTableVariable pageTableVariable)
        {
            TblEShopList = await _unitOfWork.ElectricShops.GetEShopAsync(pageTableVariable);
            Resualt.Data = new
            {
                Records = AutoMapper.Mapper.Map<List<EShop.ViewEShop>>(TblEShopList),
                Total = await _unitOfWork.ElectricShops.GetCountAsync()
            };
            return Resualt;
        }

        // search in force list
        [HttpPost]
        public JsonResult SearchInEShop(string fillterBy)
        {
            TblEShopList = _unitOfWork.ElectricShops.FindEShop(fillterBy);
            Resualt.Data = AutoMapper.Mapper.Map<List<EShop.ViewEShop>>(TblEShopList);
            return Resualt;
        }

        //check force is exist
        [HttpPost]
        public JsonResult IsExist(Guid id, string phoneNumber)
        {
            if (id != Guid.Empty)
            {
                TblEShop = _unitOfWork.ElectricShops.Single(item => item.id != id && item.managerPhoneNumber == phoneNumber);
            }
            else
            {
                TblEShop = _unitOfWork.ElectricShops.Single(item => item.managerPhoneNumber == phoneNumber);
            }
            Resualt.Data = (TblEShop != null) ? true : false;
            return Resualt;
        }

        // add or edit force
        [HttpPost]
        public async Task<JsonResult> AddEdit(tblElctricShop model)
        {
            if (model.id != Guid.Empty)
            {
                #region update
                TblEShop = await _unitOfWork.ElectricShops.SingleByIdAsync(model.id);
                TblEShop.managerPhoneNumber = model.managerPhoneNumber;
                TblEShop.managerTel = model.managerTel;
                TblEShop.shopTel = model.shopTel;
                TblEShop.address = model.address;
                TblEShop.bCode = model.bCode;
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
                    Resualt.Data = e.Message;
                }
                #endregion
            }
            else
            {
                #region Insert
                isExist = await _unitOfWork.ElectricShops
                                           .SingleAnyAsync(item =>
                                                           item.managerPhoneNumber == model.managerPhoneNumber);
                if (!isExist)
                {
                    model.id = Functions.GenerateNewId();
                    model.code = await Task.Run(() => _unitOfWork.Branches
                                                                                 .SingleById(model.bCode)
                                                                                 .code
                                                                                 + DateTime.Now.Millisecond);
                    model.code = model.code.Replace("B", "ES");
                    model.dateRegister = DateTime.Now;
                    model.pass = Functions.GenerateHash(model.managerNationalCode);
                    await Task.Run(() => _unitOfWork.ElectricShops.Add(model));
                    try
                    {
                        await _unitOfWork.CompleteAsync();
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
                else
                {
                    Resualt.Data = "این نیرو قبلا ثبت شده است.";
                }
                #endregion
            }
            return Resualt;
        }

        // disable a electric shop
        [HttpPost]
        public JsonResult DisableEShop(Guid id)
        {
            TblEShop = _unitOfWork.ElectricShops.SingleById(id);
            TblEShop.status = TblEShop.status ? false : true;
            _unitOfWork.Complete();
            try
            {
                Resualt.Data = "عملیات با موفقیت انجام شد.";
            }
            catch (Exception e)
            {
                Resualt.Data = e.Message;
            }
            return Resualt;
        }
        #endregion

        #region EShop Documents 
        //select all electric shop documents
        public JsonResult DocList(Guid id)
        {
            TblDocList = _unitOfWork.Documents
                                    .Find(item => item.elctricShopId == id)
                                    .ToList();
            ViewDocList = Mapper.Map<List<Common.ViewTblDoc>>(TblDocList);
            Resualt.Data = ViewDocList;
            return Resualt;
        }

        //add or edit force expertise
        public async Task<JsonResult> AddEditEShopDoc()
        {
            tblDocuments model = new tblDocuments
            {
                id = Guid.Parse(Request.Form[0]),
                elctricShopId = Guid.Parse(Request.Form[1]),
                title = Request.Form[2]
            };
            #region edit mode
            if (model.id != Guid.Empty)
            {
                TblDocuments = await _unitOfWork.Documents.SingleByIdAsync(model.id);
                TblDocuments.title = model.title;
                try
                {
                    await _unitOfWork.CompleteAsync();
                    Resualt.Data = "عملیات با موفقیت انجام شد.";
                }
                catch (Exception e)
                {
                    Resualt.Data = e.Message;
                }
            }
            #endregion
            #region add mode
            else
            {
                HttpPostedFileBase img = Request.Files[0];
                if (img.FileName != "")
                {
                    if (img.ContentLength <= 102400)
                    {
                        if (img.ContentType == "image/jpg" || img.ContentType == "image/jpeg")
                        {
                            model.id = Functions.GenerateNewId();
                            temp = img.ContentType.Split('/')[1];
                            temp = $"/Files/Force/PersonalPic/{model.id}.{temp}";
                            model.path = temp;
                            await Task.Run(() => _unitOfWork.Documents.Add(model));
                            try
                            {
                                await _unitOfWork.CompleteAsync();
                                img.SaveAs(Server.MapPath(temp));
                                Resualt.Data = "عملیات با موفقیت انجام شد.";
                            }
                            catch (Exception e)
                            {
                                Resualt.Data = e.Message;
                            }
                        }
                        else
                        {
                            Resualt.Data = "نوع تصویر انتخاب شده قابل قبول نمی باشد";
                        }
                    }
                    else
                    {
                        Resualt.Data = "سایز تصویر بیش از حد مجاز می باشد";
                    }
                }
                else
                {
                    Resualt.Data = "لطفا تصویر موردنظر را انتخاب کنید";
                }
            }
            #endregion
            return Resualt;
        }

        //delete force expertise 
        public JsonResult DeleteEShopDoc(Guid id)
        {
            TblDocuments = _unitOfWork.Documents.SingleById(id);
            _unitOfWork.Documents.Remove(TblDocuments);
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