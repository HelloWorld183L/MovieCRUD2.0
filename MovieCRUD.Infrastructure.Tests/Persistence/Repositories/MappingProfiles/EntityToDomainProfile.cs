using AutoMapper;
using MovieCRUD.Infrastructure.Entities;
using MovieCRUD.Domain;
using MovieCRUD.Domain.Movies;

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
