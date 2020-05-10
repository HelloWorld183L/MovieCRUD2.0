using MovieCRUD.Infrastructure.IoC;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MovieCRUD.Authentication
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ContainerConfig.RegisterAuthApiContainer(Assembly.GetExecutingAssembly(), GlobalConfiguration.Configuration);
        }
    }
}
