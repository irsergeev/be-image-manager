using ImageManager.NHibernate.Data;
using NHibernate.Mapping.ByCode;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;

namespace ImageManager.NHibernate.MapConfigurations
{
	public class ImageMap : ClassMapping<ImageDto>
	{
		public ImageMap()
		{
			Table("Image");

			Id(x => x.Id, x =>
			{
				x.Generator(Generators.Increment);
				x.Type(NHibernateUtil.Int64);
				x.Column(nameof(ImageDto.Id));
			});

			Property(b => b.Title, x =>
			{
				x.Length(50);
				x.Type(NHibernateUtil.String);
				x.NotNullable(false);
				x.Column(nameof(ImageDto.Title));
			});

			Property(b => b.Description, x =>
			{
				x.Length(200);
				x.Type(NHibernateUtil.String);
				x.NotNullable(false);
				x.Column(nameof(ImageDto.Description));
			});

			Property(b => b.FileName, x =>
			{
				x.Type(NHibernateUtil.String);
				x.NotNullable(false);
				x.Column(nameof(ImageDto.FileName));
			});

			Property(b => b.FileKey, x =>
			{
				x.Length(50);
				x.Type(NHibernateUtil.String);
				x.NotNullable(false);
				x.Column(nameof(ImageDto.FileKey));
			});

			Property(b => b.CreatedAt, x =>
			{
				x.Type(NHibernateUtil.DateTime);
				x.NotNullable(false);
				x.Column(nameof(ImageDto.CreatedAt));
			});
		}
	}
}
