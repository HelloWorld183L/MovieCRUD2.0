using AutoMapper;
using MovieCRUD.Contracts.V1.Requests.Queries;
using MovieCRUD.Domain.Filters;

namespace MovieCRUD.Api.Tests.MappingProfiles
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