using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Stylet;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MovieCRUD.Desktop.Configuration
{
    public class IoCBootstrapper<TRootViewModel> : BootstrapperBase where TRootViewModel : class
    {
        private IServiceProvider serviceProvider;
        private object _rootViewModel;
        protected virtual object RootViewModel
        {
            get { return this._rootViewModel ?? (this._rootViewModel = this.GetInstance(typeof(TRootViewModel))); }
        }

        protected override void ConfigureBootstrapper()
        {
            var serviceCollection = new ServiceCollection();
            this.ConfigureIoC(serviceCollection);
            this.serviceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void Configure()
        {
            var viewManager = (ViewManager)GetInstance(typeof(IViewManager));
            viewManager.NamespaceTransformations.Add("MovieCRUD.Desktop.ViewModels", "MovieCRUD.Desktop.Views");
            viewManager.ViewFactory = GetInstance;
            viewManager.ViewAssemblies = new List<Assembly>() { this.GetType().Assembly };
        }

        protected virtual void ConfigureIoC(IServiceCollection services) 
        {
            services.InstallTypesInAssembly();
            services.AddAutoMapper(this.GetType().Assembly);
        }

        public override object GetInstance(Type serviceType) => serviceProvider.GetService(serviceType);

        protected override void Launch()
        {
            base.DisplayRootView(this.RootViewModel);
        }
    }
}