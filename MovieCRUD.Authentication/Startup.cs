using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MovieCRUD.Authentication.Startup))]

namespace MovieCRUD.Authentication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
