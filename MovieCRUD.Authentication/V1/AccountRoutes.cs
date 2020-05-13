namespace MovieCRUD.Authentication.V1
{
    public static class AccountRoutes
    {
        private const string Root = "api/";
        private const string Version = "v1/";

        public const string Prefix = Root + Version + "Account";
        public const string AuthorizeEndPoint = "/" + Prefix + "/" + GetExternalLogin;

        public const string GetUserInfo = "GetUserInfo";
        public const string Logout = "Logout";
        public const string GetManageInfo = "GetManageInfo";
        public const string ChangePassword = "ChangePassword";
        public const string SetPassword = "SetPassword";
        public const string AddExternalLogin = "AddExternalLogin";
        public const string RemoveLogin = "RemoveLogin";
        public const string GetExternalLogin = "GetExternalLogin";
        public const string GetExternalLogins = "GetExternalLogins";
        public const string GetLogins = "GetLogins";
        public const string RegisterUser = "RegisterUser";
        public const string RegisterUserExternal = "RegisterUserExternal";
        public const string RequestToken = "/" + Prefix + "/" + "RequestToken";
        public const string ConfirmEmail = "ConfirmEmail";
        public const string SendEmail = "SendEmail";
        public const string FindByName = "FindByName";
        public const string GeneratePasswordResetToken = "GeneratePasswordResetToken";
        public const string IsEmailConfirmed = "IsEmailConfirmed";
        public const string ExternalSignIn = "ExternalSignIn";
        public const string SendTwoFactor = "SendTwoFactor";
        public const string GetValidTwoFactorProviderAsync = "GetValidTwoFactorProviderAsync";

        public const string GetTwoFactorEnabled = "GetTwoFactorEnabled";
        public const string GetUserById = "GetUserById";
        public const string GetPhoneNumber = "GetPhoneNumber";
        public const string SignIn = "SignIn";
        public const string AddLogin = "AddLogin";
        public const string GenerateChangePhoneNumberToken = "GenerateChangePhoneNumberToken";
        public const string SetTwoFactorEnabled = "SetTwoFactorEnabled";
        public const string AddPassword = "AddPassword";
        public const string SetPhoneNumber = "SetPhoneNumber";
        public const string ChangePhoneNumber = "ChangePhoneNumber";
        public const string TwoFactorSignIn = "TwoFactorSignIn";
        public const string GetExternalLoginInfo = "GetExternalLoginInfo";
        public const string GetVerifiedUserId = "GetVerifiedUserId";
    }
}