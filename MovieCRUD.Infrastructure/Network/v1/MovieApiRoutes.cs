namespace MovieCRUD.Infrastructure
{
    public static class MovieApiRoutes
    {
        private const string Root = "api/";
        private const string Version = "v1/";

        public static class MovieRoutes
        {
            public const string MovieBase = Root + Version + "Movie";
            public const string Get = MovieBase + "/{movieId}";
            public const string GetAll = MovieBase;
            public const string Post = MovieBase;
            public const string Put = MovieBase;
            public const string Delete = MovieBase + "/{movieId}";
        }
    }
}