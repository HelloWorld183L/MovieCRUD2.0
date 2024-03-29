﻿using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MovieCRUD.Authentication.App_Start;

namespace MovieCRUD.Movies
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            ContainerConfig.RegisterContainer();
        }
    }
}
