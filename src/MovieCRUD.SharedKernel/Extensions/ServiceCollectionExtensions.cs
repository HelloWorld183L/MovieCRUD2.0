﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace MovieCRUD.SharedKernel
{
    public static class ServiceCollectionExtensions
    {
        public static void InstallTypesInAssembly(this IServiceCollection services)
        {
            var installers = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetInterfaces().Contains(typeof(IInstaller)))
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();

            var orderedInstallers = installers.OrderBy(installer => installer.InstallOrder).ToList();
            orderedInstallers.ForEach(installer => installer.InstallTypes(services));
        }

        public static void InstallGeneralTypes(this IServiceCollection services, Assembly profileAssembly)
        {
            services.AddAutoMapper(profileAssembly);
        }
    }
}
