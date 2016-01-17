using System;
using System.IO;
using Castle.Facilities.Logging;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using NCmdLiner.SolutionCreator.Library.BootStrap;
using NCmdLiner.SolutionCreator.Library.Common;
using NCmdLiner.SolutionCreator.Library.Services;
using NCmdLiner.SolutionCreator.Library.ViewModels;
using NCmdLiner.SolutionCreator.Library.Views;

namespace NCmdLiner.SolutionCreator.BootStrap
{
    public class ContainerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IWindsorContainer>().Instance(container));
            container.AddFacility<TypedFactoryFacility>();
            container.Register(Component.For<ITypedFactoryComponentSelector>().ImplementedBy<CustomTypeFactoryComponentSelector>());

            //Configure logging
            var logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");
            IConfiguration configuration = new Configuration(new TemplatePath(), logger);
            log4net.GlobalContext.Properties["LogFile"] = Path.Combine(configuration.LogDirectoryPath, configuration.LogFileName);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            var applicationRootNameSpace = typeof (Program).Namespace;

            container.AddFacility<LoggingFacility>(f => f.UseLog4Net().ConfiguredExternally());
            container.Kernel.Register(Component.For<ILog>().Instance(LogManager.GetLogger(applicationRootNameSpace))); //Default logger
            container.Kernel.Resolver.AddSubResolver(new LoggerSubDependencyResolver()); //Enable injection of class specific loggers

            //Manual registrations
            container.Register(Component.For<MainWindow>().Activator<StrictComponentActivator>());
            container.Register(Component.For<MainView>().Activator<StrictComponentActivator>());
            container.Register(Component.For<MainViewModel>().Activator<StrictComponentActivator>());

            container.Register(Component.For<ISelectSolutionTemplateWindowFactory>().AsFactory());
            container.Register(
                Component.For<SelectSolutionTemplateWindow>()
                    .ImplementedBy<SelectSolutionTemplateWindow>()
                    .Named(typeof(SelectSolutionTemplateWindow).Name)
                    .LifeStyle.Transient);
            container.Register(Component.For<SelectSolutionTemplateView>().Activator<StrictComponentActivator>());
            container.Register(Component.For<SelectSolutionTemplateViewModel>().Activator<StrictComponentActivator>());

            container.Register(Component.For<ISolutionInfoWindowFactory>().AsFactory());
                        container.Register(
                            Component.For<SolutionInfoWindow>()
                                .ImplementedBy<SolutionInfoWindow>()
                                .Named(typeof(SolutionInfoWindow).Name)
                                .LifeStyle.Transient);
                        container.Register(Component.For<SolutionInfoView>().Activator<StrictComponentActivator>());
                        container.Register(Component.For<SolutionInfoViewModel>().Activator<StrictComponentActivator>());
            
            container.Register(Classes.FromAssemblyInThisApplication().IncludeNonPublicTypes().BasedOn<ITypeMapperConfiguration>().WithService.FromInterface());

            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            //Automatic registrations

            container.Register(Classes.FromAssemblyInThisApplication().BasedOn<CommandDefinition>().WithServiceBase());

            var libraryRootNameSpace = applicationRootNameSpace + ".Library";

            container.Register(Classes.FromAssemblyInThisApplication()
                .InNamespace(libraryRootNameSpace, true)
                .If(type => Attribute.IsDefined(type, typeof(SingletonAttribute)))
                .WithService.DefaultInterfaces().LifestyleSingleton());

            container.Register(Classes.FromAssemblyInThisApplication()
                .InNamespace(libraryRootNameSpace, true)
                .WithService.DefaultInterfaces().LifestyleTransient());

            IApplicationInfo applicationInfo = new ApplicationInfo();
            container.Register(Component.For<IApplicationInfo>().Instance(applicationInfo).LifestyleSingleton());
        }
    }
}
