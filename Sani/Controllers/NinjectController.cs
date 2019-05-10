using Ninject;
using ServiceLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sani.Controllers
{
    public class NinjectController : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectController()
        {
            ninjectKernel = new StandardKernel();
            AddBinding();
        }

        void AddBinding()
        {
            ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>();
        }


        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }
    }
}