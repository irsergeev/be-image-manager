using ImageManager.Core.Interfaces;
using ImageManager.Core.Mappers;
using ImageManager.Core.Models.Settings;
using ImageManager.Core.Services;
using ImageManager.NHibernate;

namespace ImageManager.Host
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var connectionString = builder.Configuration.GetConnectionString("NHibernate") ?? throw new Exception();
			builder.Services.AddNHibernate(connectionString);
			
			var storageSettings = builder.Configuration
				.GetRequiredSection(typeof(FileStoreSettings).Name)
				.Get<FileStoreSettings>() ?? throw new Exception();

			builder.Services.AddSingleton(storageSettings);
			builder.Services.AddScoped<IFileStoreProvider, FileStoreProvider>();

			builder.Services.AddAutoMapper(new[] { typeof(ImageMapper).Assembly, typeof(CommentMapper).Assembly });
			builder.Services.AddScoped<IImageService, ImageService>();

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowLocalhost", policy => policy
					.WithOrigins("*")
					.WithMethods("*")
					.WithHeaders("*"));
			});

			var app = builder.Build();

			app.UseSwagger();
			app.UseSwaggerUI();
			app.MapControllers();

			app.Run();
		}
	}
}