using ImageManager.NHibernate.Data;
using NHibernate.Mapping.ByCode;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;

namespace ImageManager.NHibernate.MapConfigurations
{
	public class CommentMap : ClassMapping<CommentDto>
	{
		public CommentMap()
		{
			Table("Comment");

			Id(x => x.Id, x =>
			{
				x.Generator(Generators.Increment);
				x.Type(NHibernateUtil.Int64);
				x.Column(nameof(CommentDto.Id));
			});

			Property(b => b.ImageId, x =>
			{
				x.Type(NHibernateUtil.Int64);
				x.NotNullable(true);
				x.Unique(false);
				x.Column(nameof(CommentDto.ImageId));
			});

			Property(b => b.Text, x =>
			{
				x.Length(200);
				x.Type(NHibernateUtil.String);
				x.NotNullable(false);
				x.Column(nameof(CommentDto.Text));
			});

			Property(b => b.CreatedAt, x =>
			{
				x.Type(NHibernateUtil.DateTime);
				x.NotNullable(false);
				x.Column(nameof(CommentDto.CreatedAt));
			});

			Property(b => b.PositionX, x =>
			{
				x.Type(NHibernateUtil.Int32);
				x.NotNullable(true);
				x.Column(nameof(CommentDto.PositionX));
			});

			Property(b => b.PositionY, x =>
			{
				x.Type(NHibernateUtil.Int32);
				x.NotNullable(true);
				x.Column(nameof(CommentDto.PositionY));
			});
		}
	}
}
