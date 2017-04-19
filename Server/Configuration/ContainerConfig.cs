using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using Server.Controllers;
using Server.Domain;
using Server.Persistence;
using Service.Persistence;
using Utilities.Entities;
using Utilities.Extensions;
using Utilities.Logging;

namespace Server.Configuration {

	internal class ContainerConfig {

		internal static void Configure(HttpConfiguration configuration) {
			Logger.Info(typeof(ContainerConfig), "Initializing container...");
			ContainerBuilder builder = new ContainerBuilder();
			builder.RegisterType<UnitOfWork>().As<IUnitOfWork<IDatabaseContext>>().InstancePerRequest().OnActivated(e => Logger.Debug("Created " + e.Instance.GetType().Name + " " + e.Instance.GetHashCode()));
			MapperConfiguration mapperConfiguration = MapperConfig.Configure();
			builder.Register(context => mapperConfiguration.CreateMapper());
			builder.RegisterAssemblyTypes(typeof(CustomerRepository).Assembly).Where(t => t.Implements(typeof(IDomainRepository))).AsImplementedInterfaces();
			builder.RegisterAssemblyTypes(typeof(CustomerRepository).Assembly).Where(t => t.Implements(typeof(IFactory))).AsImplementedInterfaces();
			builder.RegisterAssemblyTypes(typeof(CustomerRepository).Assembly).Where(t => t.Implements(typeof(IDomainService)));
			builder.RegisterApiControllers(typeof(AccountController).Assembly);
			IContainer container = builder.Build();
			configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
			Logger.Info(typeof(ContainerConfig), "Container initialized");
		}

	}

}
