namespace MovieCRUD.Movies.Requests.Queries
{
    public class GetAllByGenreQuery
    {
        public string Genre { get; set; }

        public GetAllByGenreQuery(string genre)
        {
            Genre = genre;
        }
    }
}
