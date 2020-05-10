using Autofac;
using MovieCRUD.Infrastructure.IoC;
using MovieCRUD.Desktop.ViewModels;
using Stylet;
using System;
using System.Reflection;

namespace MovieCRUD.Desktop.Configuration
{
    public class AutofacBootstrapper<TRootViewModel> : BootstrapperBase where TRootViewModel : class
    {
        private IContainer container;
        private object _rootViewModel;
        protected virtual object RootViewModel
        {
            get { return this._rootViewModel ?? (this._rootViewModel = this.GetInstance(typeof(TRootViewModel))); }
        }

        protected override void ConfigureBootstrapper()
        {
            var builder = new ContainerBuilder();
            this.ConfigureIoC(builder);
            this.container = builder.Build();
        }

        protected override void Configure()
        {
            var viewManager = (ViewManager)GetInstance(typeof(IViewManager));
            viewManager.NamespaceTransformations.Add("MovieCRUD.Desktop.ViewModels", "MovieCRUD.Desktop.Views");
        }

        protected virtual void ConfigureIoC(ContainerBuilder builder) { }

        protected void RegisterViewModels(ContainerBuilder builder, Assembly desktopAssembly)
        {
            builder.RegisterType<RegisterViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<LoginViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<MovieCRUDViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<EditMovieViewModel>().AsSelf().SingleInstance();
            builder.RegisterAssemblyTypes(desktopAssembly).ExternallyOwned();
        }

        public override object GetInstance(Type type) => container.Resolve(type);

        protected override void Launch()
        {
            base.DisplayRootView(this.RootViewModel);
        }

        public override void Dispose()
        {
            ScreenExtensions.TryDispose(this._rootViewModel);
            if (this.container != null)
                this.container.Dispose();

            base.Dispose();
        }
    }
}