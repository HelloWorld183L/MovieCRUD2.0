﻿using Microsoft.Extensions.DependencyInjection;
using MovieCRUD.Desktop.Configuration;
using MovieCRUD.Desktop.Models;
using MovieCRUD.SharedKernel;
using MovieCRUD.SharedKernel.Enums;
using Stylet;
using System.Collections.Generic;
using System.Reflection;

namespace MovieCRUD.Infrastructure.InversionOfControl.Installers
{
    public class StyletInstaller : IInstaller
    {
        public InstallOrder InstallOrder { get; set; } = InstallOrder.Trivial;

        public void InstallTypes(IServiceCollection services)
        {
            var viewManagerConfig = new ViewManagerConfig()
            {
                ViewAssemblies = new List<Assembly>() { typeof(Bootstrapper).Assembly }
            };
            var viewManager = new ViewManager(viewManagerConfig);
            var windowManager = new WindowManager(viewManager, () => new MessageBoxViewModel(), new WindowManagerConfig());

            services.AddSingleton(viewManagerConfig)
                    .AddSingleton(viewManager)
                    .AddSingleton<IEventAggregator, EventAggregator>()
                    .AddTransient<IMessageBoxViewModel, MessageBoxViewModel>()
                    .AddSingleton(new WindowManagerConfig())
                    .AddSingleton(windowManager);
        }
    }
}
