using AutoMapper;
using MovieCRUD.Contracts.V1.Responses;
using MovieCRUD.Domain;
using System.Collections.Generic;

namespace MovieCRUD.Api.Tests.MappingProfiles
{
    public class DomainResponseProfile : Profile
    {
        public DomainResponseProfile()
        {
            CreateMap<Movie, MovieResponse>().ReverseMap();
        }
    }
}