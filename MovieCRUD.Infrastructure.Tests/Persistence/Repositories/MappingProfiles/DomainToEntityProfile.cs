using AutoMapper;
using MovieCRUD.Infrastructure.Entities;
using MovieCRUD.Domain;

namespace MovieCRUD.Infrastructure.Tests.MappingProfiles
{
    public class DomainToEntityProfile : Profile
    {
        public DomainToEntityProfile()
        {
            CreateMap<Movie, MovieEntity>();
        }
    }
}
