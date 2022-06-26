using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using GE.Warehouse.Core.Configuration;
using GE.Warehouse.Core.Infrastructure.DependencyManagement;

namespace GE.Warehouse.Core.Infrastructure
{
    public class MvcEngine : IEngine
    {
        #region Fields

        private ContainerManager _containerManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates an instance of the content engine using default settings and configuration.
        /// </summary>
        public MvcEngine(HttpConfiguration apiConfig = null)
            : this(new ContainerConfigurer(), apiConfig)
        {
        }

        public MvcEngine(ContainerConfigurer mvcConfig, HttpConfiguration apiConfig)
        {
            var coreConfig = ConfigurationManager.GetSection("MvcCoreConfig") as MvcCoreConfig;
            InitializeContainer(mvcConfig, apiConfig, coreConfig);
        }

        #endregion

        #region Utilities

        private void RunStartupTasks()
        {
            var typeFinder = _containerManager.Resolve<ITypeFinder>();
            var startUpTaskTypes = typeFinder.FindClassesOfType<IStartupTask>();
            var startUpTasks = new List<IStartupTask>();
            foreach (var startUpTaskType in startUpTaskTypes)
                startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
            //sort
            startUpTasks = startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();
            foreach (var startUpTask in startUpTasks)
                startUpTask.Execute();
        }

        private void InitializeContainer(ContainerConfigurer mvcConfig, HttpConfiguration apiConfig, MvcCoreConfig coreConfig)
        {
            var builder = new ContainerBuilder();
            // Build the container.
            var container = builder.Build();

            _containerManager = new ContainerManager(container);
            mvcConfig.Configure(this, _containerManager, coreConfig);

            //builder.RegisterSource(new SettingsSource());     


            // Set the dependency resolver for MVC.
            var mvcResolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(mvcResolver);

            if (apiConfig != null)
            {
                // Create the depenedency resolver.
                var resolver = new AutofacWebApiDependencyResolver(container);
                // Configure Web API with the dependency resolver.
                apiConfig.DependencyResolver = resolver;
            }

        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize components and plugins in the nop environment.
        /// </summary>
        /// <param name="config">Config</param>
        public void Initialize(MvcCoreConfig config)
        {
            //bool databaseInstalled = DataSettingsHelper.DatabaseIsInstalled();
            //if (databaseInstalled)
            //{
            //    //startup tasks
            //    RunStartupTasks();
            //}


            //startup tasks
            if (config != null && !config.IgnoreStartupTasks)
            {
                RunStartupTasks();
            }
        }

        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        #endregion

        #region Properties

        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        #endregion
    }
}