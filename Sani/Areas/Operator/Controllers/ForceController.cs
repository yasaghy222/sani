using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ViewModel.General;
using Utility;
using System.Web.Script.Serialization;
using ViewModel.Areas;
using AutoMapper;
using ServiceLayer.Core;

namespace Sani.Areas.Operator.Controllers
{
    public class ForceController : BaseController
    {
        public ForceController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        string temp = string.Empty;
        private bool isExist = false;
        private tblCustomer TblCustomer;
        private tblForce TblForce = null;
        private tblDocuments TblDocuments = null;
        private List<tblDocuments> TblDocList = null;
        private tblForceExpertise TblForceExp = null;
        private List<Force.ViewForceExp> expList = null;
        private List<Common.ViewTblDoc> ViewDocList = null;
        private List<Force.ViewTblForce> TblForceList = null;

        [HttpGet]
        public ActionResult Index() => View();

        [HttpGet]
        public ActionResult AddEdit(Guid? id)
        {
            #region edit
            if (id != null)
            {
                TblForce = _unitOfWork.Force.SingleById(id ?? Guid.Empty);
                ViewBag.Title = $"ویرایش نیرو {TblForce.name}";
            }
            #endregion
            #region add
            else
            {
                TblForce = new tblForce
                {
                    birthDate = DateTime.Now.GetPersianDate()
                };
                ViewBag.Title = "ثبت نیروی جدید";
            }
            #endregion
            return View(model: TblForce);
        }

        #region ForceAjax
        // select all force
        [HttpPost]
        public async Task<JsonResult> ForceList(Common.PageTableVariable pageTableVariable)
        {
            TblForceList = await _unitOfWork.Force.GetForceAsync(pageTableVariable);
            Resualt.Data = new
            {
                Records = Mapper.Map<List<Force.ViewForce>>(TblForceList),
                Total = await _unitOfWork.Force.GetCountAsync()
            };
            return Resualt;
        }

        // search in force list
        [HttpPost]
        public JsonResult SearchInForce(string fillterBy)
        {
            TblForceList = _unitOfWork.Force.FindForce(fillterBy);
            Resualt.Data = AutoMapper.Mapper.Map<List<Force.ViewForce>>(TblForceList);
            return Resualt;
        }

        //check force is exist
        [HttpPost]
        public JsonResult IsExist(Guid id, string phoneNumber)
        {
            if (id != Guid.Empty)
            {
                TblForce = _unitOfWork.Force.Single(item => item.id != id && item.phoneNumber == phoneNumber);
            }
            else
            {
                TblForce = _unitOfWork.Force.Single(item => item.phoneNumber == phoneNumber);
            }
            Resualt.Data = (TblForce != null) ? true : false;
            return Resualt;
        }

        // add or edit force
        [HttpPost]
        public async Task<JsonResult> AddEdit()
        {
            tblForce model = new tblForce
            {
                id = Guid.Parse(Request.Form[0]),
                name = Request.Form[1],
                phoneNumber = Request.Form[2],
                telNumber = Request.Form[3],
                nationalCode = Request.Form[4],
                birthDate = Request.Form[5],
                address = Request.Form[6],
                bCode = Guid.Parse(Request.Form[7])
            };
            if (model.id != Guid.Empty)
            {
                #region update
                TblForce = await _unitOfWork.Force.SingleByIdAsync(model.id);
                TblForce.phoneNumber = model.phoneNumber;
                TblForce.telNumber = model.telNumber;
                TblForce.address = model.address;
                TblForce.bCode = model.bCode;
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
                #endregion
                #region Update with image
                //if (img.FileName != "")
                //{
                //    if (img.ContentLength <= 204800)
                //    {
                //        if (img.ContentType == "image/jpg" || img.ContentType == "image/jpeg")
                //        {
                //            temp = $"/Files/Force/PersonalPic/{model.id}.{img.ContentType}";
                //            model.image = temp;
                //            _unitOfWork.Force.Update(model);
                //            try
                //            {
                //                _unitOfWork.Complete();
                //                img.SaveAs(Server.MapPath(temp));
                //                System.IO.File.Delete(Server.MapPath(model.image));
                //                Resualt.Data = new
                //                {
                //                    resualt = "عملیات با موفقیت انجام شد.",
                //                    id = (model.id).ToString()
                //                };
                //            }
                //            catch (Exception e)
                //            {
                //                Resualt.Data = e.Message;
                //            }
                //        }
                //        else
                //        {
                //            Resualt.Data = "نوع تصویر انتخاب شده قابل قبول نمی باشد";
                //        }

                //    }
                //    else
                //    {
                //        Resualt.Data = "سایز تصویر بیش از حد مجاز می باشد";
                //    }
                //}
                //else
                //{
                //    model.image = await Task.Run(() => _unitOfWork.Force
                //                                                  .SingleById(model.id)
                //                                                  .image);
                //    await Task.Run(() => _unitOfWork.Force.Update(model));
                //    try
                //    {
                //        await _unitOfWork.CompleteAsync();
                //        Resualt.Data = new
                //        {
                //            resualt = "عملیات با موفقیت انجام شد.",
                //            id = (model.id).ToString()
                //        };
                //    }
                //    catch (Exception e)
                //    {
                //        Resualt.Data = e.Message;
                //    }
                //}
                #endregion
            }
            else
            {
                #region Insert
                HttpPostedFileBase img = Request.Files[0];
                isExist = await _unitOfWork.Force.SingleAnyAsync(item => item.phoneNumber == model.phoneNumber);
                if (!isExist)
                {
                    if (img.FileName != "")
                    {
                        if (img.ContentLength <= 102400)
                        {
                            if (img.ContentType == "image/jpg" || img.ContentType == "image/jpeg")
                            {
                                model.id = Functions.GenerateNewId();
                                model.mIntrodouceCode = Functions.GenerateNewCode();
                                model.personalCode = await Task.Run(() => _unitOfWork.Branches
                                                                                    .SingleById(model.bCode)
                                                                                    .code
                                                                                    + DateTime.Now.Millisecond);
                                model.personalCode = model.personalCode.Replace('B', 'F');
                                model.dateRegister = DateTime.Now;
                                model.pass = Functions.GenerateHash(model.nationalCode);
                                temp = img.ContentType.Split('/')[1];
                                temp = $"/Files/Force/PersonalPic/{model.id}.{temp}";
                                model.image = temp;
                                model.rate = 5;
                                model.status = (byte)Common.ForceStatus.offline;
                                await Task.Run(() => _unitOfWork.Force.Add(model));
                                try
                                {
                                    await _unitOfWork.CompleteAsync();
                                    img.SaveAs(Server.MapPath(temp));
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
                else
                {
                    Resualt.Data = "این نیرو قبلا ثبت شده است.";
                }
                #endregion
            }
            return Resualt;
        }

        // disable a force
        [HttpPost]
        public JsonResult DisableForce(Guid id)
        {
            TblForce = _unitOfWork.Force.SingleById(id);
            if (TblForce.status < 3)
            {
                if (TblForce.status == 0)
                {
                    TblForce.status = 1;
                }
                else
                {
                    TblForce.status = 0;
                }
                _unitOfWork.Complete();
                try
                {
                    Resualt.Data = "عملیات با موفقیت انجام شد.";
                }
                catch (Exception e)
                {
                    Resualt.Data = e.Message;
                }
            }
            else
            {
                Resualt.Data = "نمی توان نیروی مشغول به کار را معلق نمود.";
            }
            return Resualt;
        }
        #endregion

        #region Force Expertise 
        //select all force expertise
        public JsonResult ExpList(Guid id)
        {
            expList = _unitOfWork.ForceExp.ExpList(id);
            Resualt.Data = expList;
            return Resualt;
        }

        //add or edit force expertise
        public async Task<JsonResult> AddEditForceExp(tblForceExpertise model, bool formMode)
        {
            #region edit mode
            if (formMode)
            {
                _unitOfWork.ForceExp.Update(model);
            }
            #endregion
            #region add mode
            else
            {
                isExist = await _unitOfWork.ForceExp.SingleAnyAsync(item => item.catId == model.catId &&
                                                                            item.forceId == model.forceId);
                if (!isExist)
                {
                    _unitOfWork.ForceExp.Add(model);
                }
                else
                {
                    Resualt.Data = "این تخصص قبلا ثبت شده است.";
                    return Resualt;
                }
            }
            #endregion
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

        //delete force expertise 
        public JsonResult DeleteForceExp(Guid catId, Guid forceId)
        {
            TblForceExp = _unitOfWork.ForceExp
                                     .Single(item => item.catId == catId &&
                                                     item.forceId == forceId);
            _unitOfWork.ForceExp.Remove(TblForceExp);
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

        #region Force Documents 
        //select all force documents
        public JsonResult DocList(Guid id)
        {
            TblDocList = _unitOfWork.Documents
                                    .Find(item => item.forceId == id)
                                    .ToList();
            ViewDocList = Mapper.Map<List<Common.ViewTblDoc>>(TblDocList);
            Resualt.Data = ViewDocList;
            return Resualt;
        }

        //add or edit force expertise
        public async Task<JsonResult> AddEditForceDoc()
        {
            tblDocuments model = new tblDocuments
            {
                id = Guid.Parse(Request.Form[0]),
                forceId = Guid.Parse(Request.Form[1]),
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
        public JsonResult DeleteForceDoc(Guid id)
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