using AutoMapper;
using MovieCRUD.Domain.Movies;
using MovieCRUD.Infrastructure.Persistence.Entities;

namespace MovieCRUD.Infrastructure.MappingProfiles
{
    public class EntityToDomainProfile : Profile
    {
        public EntityToDomainProfile()
        {
            CreateMap<MovieEntity, Movie>();
        }
    }
}
