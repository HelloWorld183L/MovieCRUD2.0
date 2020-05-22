using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace MovieCRUD.SharedKernel
{
    public class WebApiDependencyResolver : IDependencyResolver
    {
        private IServiceProvider _serviceProvider;
        public WebApiDependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDependencyScope BeginScope()
        {
            return new WebApiDependencyResolver(_serviceProvider.CreateScope().ServiceProvider);
        }

        public void Dispose() { }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _serviceProvider.GetServices(serviceType);
        }
    }
}
