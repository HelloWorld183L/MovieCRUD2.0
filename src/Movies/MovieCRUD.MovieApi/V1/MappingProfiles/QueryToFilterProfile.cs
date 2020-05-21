using AutoMapper;
using MovieCRUD.Domain.Movies;
using MovieCRUD.Movies.Requests.Queries;
using MovieCRUD.SharedKernel;

namespace MovieCRUD.Movies.MappingProfiles
{
    public class QueryToFilterProfile : Profile
    {
        public QueryToFilterProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>().ReverseMap();
            CreateMap<GetAllByGenreQuery, GetAllByGenreFilter>().ReverseMap();
        }
    }
}