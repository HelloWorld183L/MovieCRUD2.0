using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace MovieCRUD.Authentication.Models
{
    public class ExternalLoginData
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string UserName { get; set; }
        public string ExternalAccessToken { get; set; }

        public IList<Claim> GetClaims()
        {
            IList<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

            if (UserName != null)
            {
                claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
            }

            return claims;
        }

        public static ExternalLoginData FromIdentity(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null) { return null; }

            var providerKeyClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var claimHasNoDetails = providerKeyClaim == null || string.IsNullOrEmpty(providerKeyClaim.Issuer)
                || string.IsNullOrEmpty(providerKeyClaim.Value);

            if (claimHasNoDetails) return null;
            if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer) { return null; }

            return new ExternalLoginData
            {
                LoginProvider = providerKeyClaim.Issuer,
                ProviderKey = providerKeyClaim.Value,
                UserName = claimsIdentity.FindFirstValue(ClaimTypes.Name),
                ExternalAccessToken = claimsIdentity.FindFirstValue("ExternalAccessToken")
            };
        }
    }
}