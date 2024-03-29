﻿using AutoMapper;
using MovieCRUD.DTOs;
using MovieCRUD.Movies.Responses;
using System.Collections.Generic;

namespace MovieCRUD.MappingProfiles
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