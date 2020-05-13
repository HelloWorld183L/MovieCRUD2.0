using AutoMapper;
using MovieCRUD.Domain.Movies;
using MovieCRUD.Infrastructure.Persistence.Entities;

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
