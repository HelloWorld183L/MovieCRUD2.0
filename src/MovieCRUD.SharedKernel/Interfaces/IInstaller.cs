using Microsoft.Extensions.DependencyInjection;

namespace MovieCRUD.SharedKernel
{
    public interface IInstaller
    {
        void InstallTypes(IServiceCollection services);
    }
}
