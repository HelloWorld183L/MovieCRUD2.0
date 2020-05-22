using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.SharedKernel.Enums;

namespace MovieCRUD.SharedKernel
{
    public interface IInstaller
    {
        InstallOrder InstallOrder { get; set; } 

        void InstallTypes(IServiceCollection services);
    }
}
