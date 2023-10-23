using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System.Data.Common;

namespace ImageManager.NHibernate.InitTool
{
	public class Program
	{
		private const int RETRIEVE_COUNT = 2;
		private const string CONNECTION_STRING_KEY = "--connection-string";

		static async Task Main(string[] args)
		{
			var connectionStringKey = args[0];
			var connectionString = args[1];

			if (connectionStringKey != CONNECTION_STRING_KEY)
			{
				throw new ArgumentNullException("The connection string argument is not found ");
			}

			if (string.IsNullOrEmpty(connectionString))
			{
				throw new ArgumentNullException(nameof(connectionString), "Failed to read the connection string");
			}

			int iteration = 1;
			bool successMarker = false;

			while (iteration <= RETRIEVE_COUNT || !successMarker)
			{
				try
				{
					await TryCreateDatabase(connectionString);

					var mapper = new ModelMapper();
					mapper.AddMappings(typeof(NHibernateExtension).Assembly.ExportedTypes);
					HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

					var configuration = new Configuration();
					configuration.DataBaseIntegration(c =>
					{
						c.Dialect<MsSql2012Dialect>();
						c.ConnectionString = connectionString;
						c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
						c.SchemaAction = SchemaAutoAction.Create;
					});

					configuration.AddMapping(domainMapping);

					var schemaExporter = new SchemaExport(configuration);
					schemaExporter.Execute(true, true, false);

					Console.WriteLine("\n\nInit database operation has completed successfully");
					successMarker = true;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}

				iteration = iteration + 1;

				if (iteration == RETRIEVE_COUNT)
				{
					Console.WriteLine("Can not initialize database stucture");
				}
			}
		}

		private static async Task TryCreateDatabase(string connectionString)
		{
			var connectionBuilder = new DbConnectionStringBuilder();
			connectionBuilder.ConnectionString = connectionString;

			if (!connectionBuilder.TryGetValue("Initial Catalog", out object? catalogName))
				throw new Exception("Initial Catalog is not contained in the configuration");

			if (!connectionBuilder.TryGetValue("Server", out object? server))
				throw new Exception("Server is not contained in the configuration");

			if (!connectionBuilder.TryGetValue("User Id", out object? username))
				throw new Exception("User id is not contained in the configuration");

			if (!connectionBuilder.TryGetValue("Password", out object? password))
				throw new Exception("Password is not contained in the configuration");


			var configuration = Fluently.Configure().Database(MsSqlConfiguration.MsSql2012.ConnectionString(c => c
				.Database("master")
				.Server(server.ToString())
				.Username(username.ToString())
				.Password(password.ToString())));

			var sessionFactory = configuration.BuildSessionFactory();
			using var session = sessionFactory.OpenSession();

			await session
				.CreateSQLQuery($"IF DB_ID(N'{catalogName}') IS NULL CREATE DATABASE {catalogName}")
				.ExecuteUpdateAsync();
		}
	}
}
