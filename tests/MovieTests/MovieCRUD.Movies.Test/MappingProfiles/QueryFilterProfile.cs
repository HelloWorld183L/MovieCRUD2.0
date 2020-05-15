using AutoMapper;
using MovieCRUD.Domain.Movies;
using MovieCRUD.Movies.Requests.Queries;
using MovieCRUD.SharedKernel;

namespace MovieCRUD.Movies.Tests.MappingProfiles
{
    public class QueryFilterProfile : Profile
    {
        public QueryFilterProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>().ReverseMap();
            CreateMap<GetAllByGenreQuery, GetAllByGenreFilter>().ReverseMap();
        }
    }
}