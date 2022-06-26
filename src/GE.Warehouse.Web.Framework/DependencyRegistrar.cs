using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Integration.Mvc;
using GreenSign.Services;
using GreenSign.Services.Events;
using MVCCore;
using MVCCore.Caching;
using MVCCore.Configuration;
using MVCCore.Data;
using MVCCore.Fakes;
using MVCCore.Infrastructure;
using MVCCore.Infrastructure.DependencyManagement;
using MVCCore.Plugins;
using GreenSign.Repository;
using GreenSign.Services;
using GreenSign.Web.Framework.EmbeddedViews;
using GreenSign.Web.Framework.Mvc.Routes;
using GreenSign.Web.Framework.UI;
using GreenSign.Web.Framework.UI.Editor;

namespace GreenSign.Web.Framework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //HTTP context and other related stuff
            builder.Register(c => 
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerHttpRequest();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerHttpRequest();

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerHttpRequest();

            //controllers
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            //data layer
            var dataSettingsManager = new DataSettingsManager();
            var dataProviderSettings = dataSettingsManager.LoadSettings();
            builder.Register(c => dataSettingsManager.LoadSettings()).As<DataSettings>();
            builder.Register(x => new EfDataProviderManager(x.Resolve<DataSettings>())).As<BaseDataProviderManager>().InstancePerDependency();


            builder.Register(x => (IEfDataProvider)x.Resolve<BaseDataProviderManager>().LoadDataProvider()).As<IDataProvider>().InstancePerDependency();
            builder.Register(x => (IEfDataProvider)x.Resolve<BaseDataProviderManager>().LoadDataProvider()).As<IEfDataProvider>().InstancePerDependency();

            if (dataProviderSettings != null && dataProviderSettings.IsValid())
            {
                var efDataProviderManager = new EfDataProviderManager(dataSettingsManager.LoadSettings());
                var dataProvider = (IEfDataProvider)efDataProviderManager.LoadDataProvider();
                dataProvider.InitConnectionFactory();

                builder.Register<IDbContext>(c => new DbObjectContext(dataProviderSettings.DataConnectionString)).InstancePerHttpRequest();
            }
            else
            {
                builder.Register<IDbContext>(c => new DbObjectContext(dataSettingsManager.LoadSettings().DataConnectionString)).InstancePerHttpRequest();
            }


            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerHttpRequest();
            
            //plugins
            builder.RegisterType<PluginFinder>().As<IPluginFinder>().InstancePerHttpRequest();

            //cache manager
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("webmvc_cache_static").SingleInstance();
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("webmvc_cache_per_request").InstancePerHttpRequest();

            builder.RegisterType<CommentServices>().As<ICommentServices>().InstancePerHttpRequest();
            builder.RegisterType<OrderServices>().As<IOrderServices>().InstancePerHttpRequest();
            builder.RegisterType<ProcessServices>().As<IProcessServices>().InstancePerHttpRequest();
            builder.RegisterType<ProductTypeServices>().As<IProductTypeServices>().InstancePerHttpRequest();
            builder.RegisterType<RoleServices>().As<IRoleServices>().InstancePerHttpRequest();
            builder.RegisterType<SeasonServices>().As<ISeasonServices>().InstancePerHttpRequest();
            builder.RegisterType<SignalServices>().As<ISignalServices>().InstancePerHttpRequest();
            builder.RegisterType<BrandNameServices>().As<IBrandNameServices>().InstancePerHttpRequest();
            builder.RegisterType<UserServices>().As<IUserServices>().InstancePerHttpRequest();
            builder.RegisterType<MessagesServices>().As<IMessagesServices>().InstancePerHttpRequest();
            builder.RegisterType<VendorServices>().As<IVendorServices>().InstancePerHttpRequest();
            builder.RegisterType<AuthenticationServices>().As<IAuthenticationServices>().InstancePerHttpRequest();
            builder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerHttpRequest();
            builder.RegisterType<PermissionServices>().As<IPermissionServices>().InstancePerHttpRequest();
            builder.RegisterType<RelationSuperMechandiserServices>().As<IRelationSuperMechandiserServices>().InstancePerHttpRequest();
            //register file and email
            builder.RegisterType<FileWrapper>().As<IFile>().InstancePerHttpRequest();
            builder.RegisterType<EmailSender>().As<IEmailSender>().InstancePerHttpRequest();

            //register webhelper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerHttpRequest();
            
            builder.RegisterType<EmbeddedViewResolver>().As<IEmbeddedViewResolver>().SingleInstance();
            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();

            //HTML Editor services
            builder.RegisterType<NetAdvDirectoryService>().As<INetAdvDirectoryService>().InstancePerHttpRequest();
            builder.RegisterType<NetAdvImageService>().As<INetAdvImageService>().InstancePerHttpRequest();

            //Register event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerHttpRequest();
            }
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();

        }

        public int Order
        {
            get { return 0; }
        }
    }


    public class SettingsSource : IRegistrationSource
    {
        static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);

        public IEnumerable<IComponentRegistration> RegistrationsFor(
                Service service,
                Func<Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }

        static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
        {
            return RegistrationBuilder
                .ForDelegate((c, p) => c.Resolve<IConfigurationProvider<TSettings>>().Settings)
                .InstancePerHttpRequest()
                .CreateRegistration();
        }

        public bool IsAdapterForIndividualComponents { get { return false; } }
    }

}
