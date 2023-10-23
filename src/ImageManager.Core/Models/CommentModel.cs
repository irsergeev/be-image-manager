namespace ImageManager.Core.Models
{
	public class CommentModel
	{
		public long Id { get; set; }
		public long ImageId { get; set; }
		public string Text { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
		public int PositionX { get; set; }
		public int PositionY { get; set; }
	}
}
