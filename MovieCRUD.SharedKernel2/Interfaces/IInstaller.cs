using Microsoft.Extensions.DependencyInjection;

namespace MovieCRUD.Infrastructure.InversionOfControl.Installers.Interfaces
{
    public interface IInstaller
    {
        void InstallTypes(IServiceCollection services);
    }
}
