﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieCRUD.Authentication.App_Start
{
    public class ContainerConfig
    {
        public static void RegisterContainer()
        {
            IServiceCollection services = new ServiceCollection();
            services.InstallTypesInAssembly();
            services.AddAutoMapper(typeof(ContainerConfig).Assembly);
        }
    }
}