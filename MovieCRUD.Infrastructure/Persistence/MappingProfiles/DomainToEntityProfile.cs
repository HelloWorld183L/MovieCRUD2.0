using AutoMapper;
using MovieCRUD.Infrastructure.Entities;
using MovieCRUD.Domain.Movies;

namespace MovieCRUD.Infrastructure.MappingProfiles
{
    public class DomainToEntityProfile : Profile
    {
        public DomainToEntityProfile()
        {
            CreateMap<Movie, MovieEntity>();
        }
    }
}
