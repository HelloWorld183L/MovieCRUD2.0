using AutoMapper;
using MovieCRUD.Infrastructure.Entities;
using MovieCRUD.Domain;

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
