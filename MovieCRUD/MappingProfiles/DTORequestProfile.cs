using AutoMapper;
using MovieCRUD.Contracts.V1.Requests;
using MovieCRUD.DTOs;

namespace MovieCRUD.MappingProfiles
{
    public class DTORequestProfile : Profile
    {
        public DTORequestProfile()
        {
            CreateMap<MovieDTO, EditMovieRequest>().ReverseMap();
            CreateMap<MovieDTO, CreateMovieRequest>().ReverseMap();
        }
    }
}