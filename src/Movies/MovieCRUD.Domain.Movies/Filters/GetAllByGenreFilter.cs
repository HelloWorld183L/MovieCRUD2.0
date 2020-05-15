namespace MovieCRUD.Domain.Movies
{
    public class GetAllByGenreFilter
    {
        public string Genre { get; set; }

        public GetAllByGenreFilter(string genre)
        {
            Genre = genre;
        }
    }
}
