using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using WorkScheduler.Repository;
using WorkScheduler.Service;

namespace WorkScheduler.WebAPI.App_Start
{
    public class AutofacConfig
    {
        public static void StartDI()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}