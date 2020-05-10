using AutoMapper;
using MovieCRUD.Contracts.V1.Responses;
using MovieCRUD.Desktop.Models.DTOs;
using System.Collections.Generic;

namespace MovieCRUD.Desktop.MappingProfiles
{
    public class DTOResponseProfile : Profile
    {
        public DTOResponseProfile()
        {
            CreateMap<MovieResponse, MovieDTO>().ReverseMap();
            CreateMap<IEnumerable<MovieDTO>, IEnumerable<MovieResponse>>().ReverseMap();
        }
    }
}
