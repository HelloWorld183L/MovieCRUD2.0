using MovieCRUD.SharedKernel;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InstallerExtensions
    {
        public static void InstallTypesInAssembly(this IServiceCollection services, Assembly assembly)
        {
            var installers = assembly.ExportedTypes.Where(x =>
                typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            installers.ForEach(installer => installer.InstallTypes(services));
        }
    }
}
