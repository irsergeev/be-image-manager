using ImageManager.Core.Interfaces;
using ImageManager.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageManager.Host.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Produces("application/json")]
	public class ImageController
	{
		private readonly IImageService _imageService;
		
		public ImageController(IImageService imageService) 
		{
			_imageService = imageService;
		}

		[HttpGet]
		public async Task<IEnumerable<ImageModel>> GetImages()
		{
			var result = await _imageService.GetImagesAsync();
			return result;
		}

		[HttpPost]
		public async Task<long> CreateImageAsync([FromBody] ImageModel pictureModel)
		{
			var id = await _imageService.CreateImageAsync(pictureModel);
			return id;
		}

		[HttpDelete]
		public async Task DeleteImageAsync([FromQuery] long id)
		{
			await _imageService.DeleteImageAsync(id);
		}

		[HttpPut]
		public async Task UpdateImageAsync([FromBody] ImageModel picture)
		{
			await _imageService.UpdateImageAsync(picture);
		}

		[HttpGet("{imageId}/Comment")]
		public async Task<IEnumerable<CommentModel>> GetComments(long imageId)
		{
			var comments = await _imageService.GetCommentsAsync(imageId);
			return comments;
		}

		[HttpPost("Comment")]
		public async Task<long> CreateComment([FromBody] CommentModel commentModel)
		{
			var commentId = await _imageService.CreateCommentAsync(commentModel);
			return commentId;
		}

		[HttpDelete("Comment")]
		public async Task DeleteCommentAsync([FromQuery] long id)
		{
			await _imageService.DeleteCommentAsync(id);
		}

		[HttpPut("Comment")]
		public async Task UpdateCommentAsync([FromBody] CommentModel commentModel)
		{
			await _imageService.UpdateCommentAsync(commentModel);
		}
	}
}
