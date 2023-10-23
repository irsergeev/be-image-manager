using ImageManager.NHibernate.Interfaces;

namespace ImageManager.NHibernate.Data
{
	public class ImageDto : IIdentityEntity<long>
	{
		public virtual long Id { get; set; }
		public virtual string? Title { get; set; } = string.Empty;
		public virtual string? Description { get; set; } = string.Empty;
		public virtual DateTime CreatedAt { get; set; }
		public virtual string FileName { get; set; } = string.Empty;
		public virtual string FileKey { get; set; } = string.Empty;
	}
}
