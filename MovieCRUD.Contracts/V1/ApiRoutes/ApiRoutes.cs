namespace MovieCRUD.Contracts.V1.ApiRoutes
{
    public static class ApiRoutes
    {
        private const string Root = "api/";
        private const string Version = "v1/";

        public static class MovieRoutes
        {
            private const string MovieBase = Root + Version + "Movie";

            public const string Get = MovieBase + "/{movieId}";
            public const string GetAll = MovieBase;
            public const string Post = MovieBase;
            public const string Put = MovieBase;
            public const string Delete = MovieBase + "/{movieId}";
        }

        public static class AccountRoutes
        {
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
            public const string RegisterUser = "RegisterUser";
            public const string RegisterUserExternal = "RegisterUserExternal";
            public const string RequestToken = "/" + Prefix + "/" + "RequestToken";
        }
    }
}
