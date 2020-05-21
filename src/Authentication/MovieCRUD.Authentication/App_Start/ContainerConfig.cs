﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MovieCRUD.Authentication.App_Start
{
    public class ContainerConfig
    {
        public static void RegisterContainer()
        {
            IServiceCollection services = new ServiceCollection();
            var executingAssembly = Assembly.GetExecutingAssembly();
            services.InstallTypesInAssembly(executingAssembly);
            services.AddAutoMapper(executingAssembly);
        }
    }
}