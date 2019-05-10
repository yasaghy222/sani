using ServiceLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sani.Areas.Operator.Controllers
{
    public class DashboardController : BaseController
    {

        public DashboardController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        // GET: Operator/Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard() => View();
    }
}