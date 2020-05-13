using AutoMapper;
using MovieCRUD.Domain.Movies;
 
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
