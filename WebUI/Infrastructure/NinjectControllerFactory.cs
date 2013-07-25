using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Domain.Abstract;
using Domain.Concrete;  
using WebUI.Infrastructure.Abstract;
using WebUI.Infrastructure.Concrete;
using System.Web.Security;
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
            ninjectKernel.Bind<IUserRepository>().To<EFUserRepository>();
            ninjectKernel.Bind<IUserRolesRepository>().To<EFUserRoleRepository>();
            ninjectKernel.Bind<IAuthenticateProvider>().To<AuthenticateProvider>();
            ninjectKernel.Bind<ICompaniesRepository>().To<EFCompanyRepository>();
            ninjectKernel.Bind<IEquipmentTypesRepository>().To<EFEquipmentRepository>();
            ninjectKernel.Bind<IOrdersRepository>().To<EFOrderRepository>();
            ninjectKernel.Bind<IReperairType>().To<EFReperairType>();
            ninjectKernel.Bind<IReperairTypePrice>().To<EFReperairTypePrice>();
            ninjectKernel.Bind<IOrderStatusesRepository>().To<EFOrderStatusesRepository>();
        }
    }
}