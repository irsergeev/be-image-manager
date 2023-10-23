using ImageManager.NHibernate.Interfaces;

namespace ImageManager.NHibernate.Data
{
	public class CommentDto : IIdentityEntity<long>
	{
		public virtual long Id { get; set; }
		public virtual long ImageId { get; set; }
		public virtual string Text { get; set; } = string.Empty;
		public virtual DateTime CreatedAt { get; set; }
		public virtual int PositionX { get; set; }
		public virtual int PositionY { get; set; }
	}
}
