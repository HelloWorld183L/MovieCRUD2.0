using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using MovieCRUD.Infrastructure.IoC;

namespace MovieCRUD.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            ContainerConfig.RegisterMovieApiContainer(Assembly.GetExecutingAssembly(), GlobalConfiguration.Configuration);
        }
    }
}
