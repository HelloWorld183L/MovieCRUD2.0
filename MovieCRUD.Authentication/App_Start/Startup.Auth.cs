using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using MovieCRUD.Authentication.Providers;
using MovieCRUD.Contracts.V1.ApiRoutes;
using MovieCRUD.Infrastructure;

namespace MovieCRUD.Authentication
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder appBuilder)
        {
            appBuilder.CreatePerOwinContext(() => new ApplicationDbContext());
            appBuilder.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            appBuilder.UseCookieAuthentication(new CookieAuthenticationOptions());
            appBuilder.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString(ApiRoutes.AccountRoutes.RequestToken),
                Provider = new ApplicationOAuthProvider(),
                AuthorizeEndpointPath = new PathString(ApiRoutes.AccountRoutes.AuthorizeEndPoint),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };

            appBuilder.UseOAuthBearerTokens(OAuthOptions);

            ConfigureIdentityProviders(appBuilder);
        }

        private void ConfigureIdentityProviders(IAppBuilder appBuilder)
        {
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}
