using ImageManager.NHibernate.Data;
using ImageManager.Repository;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Dialect;

namespace ImageManager.NHibernate
{
	public static class NHibernateExtension
	{
		public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString)
		{
			var mapper = new ModelMapper();
			mapper.AddMappings(typeof(NHibernateExtension).Assembly.ExportedTypes);
			HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

			var configuration = new Configuration();
			configuration.DataBaseIntegration(c =>
			{
				c.Dialect<MsSql2012Dialect>();
				c.ConnectionString = connectionString;
				c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
				c.SchemaAction = SchemaAutoAction.Validate;
			});

			configuration.AddMapping(domainMapping);

			ISessionFactory sessionFactory = null;

			try
			{
				sessionFactory = configuration.BuildSessionFactory();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex);
			}

			services.AddSingleton(sessionFactory);

			services.AddScoped(factory => sessionFactory.OpenSession());
			services.AddScoped<IRepository<ImageDto, long>, NHibernateRepository<ImageDto, long>>();
			services.AddScoped<IRepository<CommentDto, long>, NHibernateRepository<CommentDto, long>>();

			return services;
		}
	}
}
