using AutoMapper;
using MovieCRUD.Contracts.V1.Requests.Queries;
using MovieCRUD.Domain.Filters;

namespace MovieCRUD.Api.MappingProfiles
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