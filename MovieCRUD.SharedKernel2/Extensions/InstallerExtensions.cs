using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.Infrastructure.InversionOfControl.Installers.Interfaces;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InstallerExtensions
    {
        public static void InstallTypesInAssembly(this IServiceCollection services)
        {
            var installers = 
                AppDomain.
                CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetInterfaces().Contains(typeof(IInstaller)))
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();

            installers.ForEach(installer => installer.InstallTypes(services));
        }
    }
}
