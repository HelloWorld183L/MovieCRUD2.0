using AutoMapper;
using MovieCRUD.Infrastructure.Entities;
using MovieCRUD.Domain;

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
