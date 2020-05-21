using AutoMapper;
using MovieCRUD.Domain;
using MovieCRUD.Domain.Movies;
using MovieCRUD.Movies.Requests;

namespace MovieCRUD.Movies.MappingProfiles
{
    public class RequestDomainProfile : Profile
    {
        public RequestDomainProfile()
        {
            CreateMap<CreateMovieRequest, Movie>().ReverseMap();
            CreateMap<EditMovieRequest, Movie>().ReverseMap();
        }
    }
}