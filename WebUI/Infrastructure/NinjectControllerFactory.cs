using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;

namespace WebUI.Infrastructure
{
    //ninjectControllerFactory will be use to create controllers
    public class NinjectControllerFactory :DefaultControllerFactory
    {
        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }
        
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null :(IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            //binding place
        }
    }
}