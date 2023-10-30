using System.Text.Json.Serialization;

namespace ImageManager.Core.Models
{
	public class ImageModel
	{
		public long Id { get; set; }

		public string FileName { get; set; } = string.Empty;

		public string Title { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; }

		[JsonIgnore]
		public string FileKey { get; set; } = string.Empty;

		public string FileDataAsString { get; set; } = string.Empty;

		public ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
	}
}
