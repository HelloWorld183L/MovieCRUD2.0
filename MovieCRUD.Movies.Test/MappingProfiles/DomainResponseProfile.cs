using AutoMapper;
using MovieCRUD.Domain.Movies;
using MovieCRUD.Movies.Responses;

namespace MovieCRUD.Movies.Tests.MappingProfiles
{
    public class DomainResponseProfile : Profile
    {
        public DomainResponseProfile()
        {
            CreateMap<Movie, MovieResponse>().ReverseMap();
        }
    }
}