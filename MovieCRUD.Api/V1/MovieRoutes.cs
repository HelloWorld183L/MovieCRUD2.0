namespace MovieCRUD.Movies.V1
{
    public static class MovieRoutes
    {
        private const string Root = "api/";
        private const string Version = "v1/";

        private const string MovieBase = Root + Version + "Movie";

        public const string Get = MovieBase;
        public const string GetAll = MovieBase;
        public const string Post = MovieBase;
        public const string Put = MovieBase;
        public const string Delete = MovieBase + "/{movieId}";
    }
}