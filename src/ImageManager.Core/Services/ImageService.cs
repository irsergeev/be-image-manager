using AutoMapper;
using ImageManager.Core.Interfaces;
using ImageManager.Core.Models;
using ImageManager.NHibernate.Data;
using ImageManager.Repository;

namespace ImageManager.Core.Services
{
	public class ImageService : IImageService
	{
		private readonly IRepository<ImageDto, long> _picturesRepository;
		private readonly IRepository<CommentDto, long> _commentRepository;
		private readonly IFileStoreProvider _fileStoreProvider;
		private readonly IMapper _mapper;

		public ImageService(
			IRepository<ImageDto, long> pictureRepository,
			IRepository<CommentDto, long> commentRepository,
			IFileStoreProvider fileStoreProvider,
			IMapper mapper)
		{
			_picturesRepository = pictureRepository;
			_commentRepository = commentRepository;
			_fileStoreProvider = fileStoreProvider;
			_mapper = mapper;
		}

		public async Task<IEnumerable<ImageModel>> GetImagesAsync()
		{
			var result = await _picturesRepository.GetAllAsync();
			var pictures = _mapper.Map<IEnumerable<ImageModel>>(result);

			foreach (var picture in pictures)
			{
				var imageData = await _fileStoreProvider.GetFileAsync(picture.FileKey);
				picture.FileDataAsString = imageData;
			}

			return pictures;
		}

		public async Task<long> CreateCommentAsync(CommentModel comment)
		{
			var commentDto = _mapper.Map<CommentDto>(comment);
			var commentIdentity = await _commentRepository.CreateAsync(commentDto);

			return commentIdentity;
		}

		public async Task<long> CreateImageAsync(ImageModel image)
		{
			if (string.IsNullOrWhiteSpace(image.FileDataAsString))
			{
				throw new Exception();
			}

			if (string.IsNullOrWhiteSpace(image.Description))
			{
				throw new Exception();
			}


			string fileKey = string.Empty;
			image.CreateAt = DateTime.Now;

			try
			{
				fileKey = await _fileStoreProvider.CreateFileAsync(image.FileDataAsString);
				image.FileKey = fileKey;

				var imageDto = _mapper.Map<ImageDto>(image);
				var pictureIdentity = await _picturesRepository.CreateAsync(imageDto);

				return pictureIdentity;
			}
			catch (Exception ex)
			{
				await _fileStoreProvider.DeleteFileAsync(fileKey);
				throw new Exception(ex.Message, ex);
			}
		}

		public async Task UpdateCommentAsync(CommentModel comment)
		{
			var dbComment = _mapper.Map<CommentDto>(comment);
			await _commentRepository.UpdateAsync(dbComment);
		}

		public async Task UpdateImageAsync(ImageModel image)
		{
			var dbImage = await _picturesRepository.GetAsync(image.Id);

			if (dbImage != null)
			{
				dbImage.Title = image.Title;
				dbImage.Description = image.Description;

				await _picturesRepository.UpdateAsync(dbImage);
			}
		}

		public async Task DeleteCommentAsync(long commentId)
		{
			await _commentRepository.DeleteAsync(commentId);
		}

		public async Task DeleteImageAsync(long imageId)
		{
			var dbImage = await _picturesRepository.GetAsync(imageId);
			
			if (dbImage != null)
			{
				await _fileStoreProvider.DeleteFileAsync(dbImage.FileKey);
				await DeleteCommentsAsync(imageId);
				await _picturesRepository.DeleteAsync(imageId);
			}
		}

		public async Task<IEnumerable<CommentModel>> GetCommentsAsync(long imageId)
		{
			var result = await _commentRepository.FindAsync(c => c.ImageId == imageId);
			var comments = _mapper.Map<IEnumerable<CommentModel>>(result);

			return comments;
		}

		public async Task DeleteCommentsAsync(long imageId)
		{
			await _commentRepository.DeleteAsync(c => c.ImageId == imageId);
		}
	}
}
