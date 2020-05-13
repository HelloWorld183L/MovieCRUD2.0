using AutoMapper;
using MovieCRUD.Domain;
using MovieCRUD.Domain.Movies;
using MovieCRUD.Infrastructure.Persistence.Entities;

namespace MovieCRUD.Infrastructure.Tests.MappingProfiles
{
    public class EntityToDomainProfile : Profile
    {
        public EntityToDomainProfile()
        {
            CreateMap<MovieEntity, Movie>();
        }
    }
}
