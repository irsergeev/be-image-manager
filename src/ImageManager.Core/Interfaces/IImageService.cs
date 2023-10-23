using ImageManager.Core.Models;

namespace ImageManager.Core.Interfaces
{
	public interface IImageService
	{
		Task<IEnumerable<ImageModel>> GetImagesAsync();
		Task<long> CreateImageAsync(ImageModel image);
		Task DeleteImageAsync(long imageId);
		Task UpdateImageAsync(ImageModel image);

		Task<IEnumerable<CommentModel>> GetCommentsAsync(long imageId);
		Task<long> CreateCommentAsync(CommentModel comment);
		Task DeleteCommentAsync(long commentId);
		Task UpdateCommentAsync(CommentModel comment);
	}
}
