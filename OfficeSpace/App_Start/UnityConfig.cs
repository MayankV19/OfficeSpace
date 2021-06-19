using OfficeSpace.BussinessService;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using OfficeSpace.Services;

namespace OfficeSpace
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IRequestBusinessService, RequestBusinessService>();
          
            container.RegisterType<INavigationRepositoryService, NavigationRepositoryService>();

            container.RegisterType<IMasterBussinessService, MasterBusinessService>();

            container.RegisterType<IMasterRepositoryService, MasterRepositoryService>();
            container.RegisterType<ILogService, LogService>();
            container.RegisterType<IReportBussinessService, ReportBusinessService>();
            container.RegisterType<IReportRepositorySerivce, ReportRepositoryService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }

    internal interface IReportsBussinessService
    {
    }
}