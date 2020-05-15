using AutoMapper;
using MovieCRUD.Domain.Movies;
using MovieCRUD.Movies.Requests;

namespace MovieCRUD.Movies.Tests.MappingProfiles
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