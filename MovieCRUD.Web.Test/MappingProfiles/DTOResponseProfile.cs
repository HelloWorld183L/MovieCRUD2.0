using AutoMapper;
using MovieCRUD.Contracts.V1.Responses;
using MovieCRUD.DTOs;
using System.Collections.Generic;

namespace MovieCRUD.Web.Tests.MappingProfiles
{
    public class DTOResponseProfile : Profile
    {
        public DTOResponseProfile()
        {
            CreateMap<MovieDTO, MovieResponse>().ReverseMap();
            CreateMap<IEnumerable<MovieResponse>, IEnumerable<MovieDTO>>().ReverseMap();
        }
    }
}