namespace MovieCRUD.Contracts.V1.Requests
{
    public class EditMovieRequest : DefaultMovieRequest
    {
        public int Id { get; set; }
    }
}