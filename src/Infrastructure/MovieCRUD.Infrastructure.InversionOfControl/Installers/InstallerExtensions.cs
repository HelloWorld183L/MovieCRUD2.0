using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.Infrastructure.InversionOfControl.Installers.Interfaces;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InstallerExtensions
    {
        public static IServiceCollection InstallTypesInAssembly(this IServiceCollection services)
        {
            var installers = typeof(InstallerExtensions).Assembly.ExportedTypes.Where(installerType =>
                typeof(IInstaller).IsAssignableFrom(installerType) && !installerType.IsAbstract && !installerType.IsInterface)
                    .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            installers.ForEach(installer => installer.InstallTypes(services));
            return services;
        }
    }
}
