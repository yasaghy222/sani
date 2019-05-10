using System;
using System.ComponentModel.DataAnnotations;

namespace ViewModel.General
{
    public class Common
    {
        public class PageTableVariable
        {
            public int PageSize { get; set; }

            public int PageIndex { get; set; }

            public int OrderBy { get; set; }

            /// <summary>
            /// false is desc and true is asc
            /// </summary>
            public bool OrderType { get; set; }
        }

        public class ViewTblDoc
        {
            public Guid id { get; set; }
            public Guid? bCode { get; set; }
            public Guid? electricShopId { get; set; }
            public Guid? forceId { get; set; }
            public string title { get; set; }
            public string path { get; set; }
        }

        public enum FactorStatus : byte
        {
            /// <summary>
            /// پیش فاکتور
            /// </summary>
            [Display(Name = "پیش فاکتور")]
            preFactor = 0,

            /// <summary>
            /// رزرو
            /// </summary>
            [Display(Name = "رزرو")]
            reserve = 1,

            /// <summary>
            /// در جستجوی نیرو
            /// </summary>
            [Display(Name = "در جستجوی نیرو")]
            findForce = 2,

            /// <summary>
            /// تایید نیرو
            /// </summary>
            [Display(Name = "تایید نیرو")]
            forceConfirm = 3,

            /// <summary>
            /// در حال انجام
            /// </summary>
            [Display(Name = "در حال انجام")]
            inProcess = 4,

            /// <summary>
            /// معلق
            /// </summary>
            [Display(Name = "معلق")]
            suspend = 5,

            /// <summary>
            /// ویرایش شده
            /// </summary>
            [Display(Name = "ویرایش شده")]
            isChange = 6,

            /// <summary>
            /// تایید ویرایش
            /// </summary>
            [Display(Name = "تایید ویرایش")]
            changeConfirm = 7,

            /// <summary>
            /// اتمام
            /// </summary>
            [Display(Name = "اتمام")]
            finish = 8,

            /// <summary>
            /// لغو شده توسط کاربر
            /// </summary>
            [Display(Name = "لغو شده توسط کاربر")]
            cancelByUser = 9,

            /// <summary>
            /// لغو شده توسط نیرو
            /// </summary>
            [Display(Name = "لغو شده توسط نیرو")]
            cancelByForce = 10
        }
        public enum ForceStatus : byte
        {
            /// <summary>
            /// معلق
            /// </summary>
            [Display(Name = "معلق")]
            suspend = 0,

            /// <summary>
            /// آفلاین
            /// </summary>
            [Display(Name = "آفلاین")]
            offline = 1,

            /// <summary>
            /// آماده به کار
            /// </summary>
            [Display(Name = "آماده به کار")]
            ready = 2,

            /// <summary>
            /// مشغول به کار
            /// </summary>
            [Display(Name = "مشغول به کار")]
            atWork = 3
        }
    }
}
