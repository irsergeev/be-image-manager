using ImageManager.Core.Interfaces;
using ImageManager.Core.Models.Settings;
using System.Text;

namespace ImageManager.Core.Services
{
	public class FileStoreProvider : IFileStoreProvider
	{
		private readonly string _storagePath;

		public FileStoreProvider(FileStoreSettings storeSettings)
		{
			_storagePath = storeSettings.PathToStorage;

			if (!Directory.Exists(_storagePath))
			{
				Directory.CreateDirectory(_storagePath);
			}
		}

		public async Task<string> CreateFileAsync(string fileData)
		{
			var fileKey = Guid.NewGuid().ToString();
			var filePath = Path.Combine(_storagePath, fileKey);

			try
			{
				var bytes = Encoding.ASCII.GetBytes(fileData);
				await File.WriteAllBytesAsync(filePath, bytes);
				return fileKey;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex);
			}
		}

		public Task DeleteFileAsync(string fileKey)
		{
			var filePath = Path.Combine(_storagePath, fileKey);

			if (File.Exists(filePath))
			{
				File.Delete(filePath);
				return Task.CompletedTask;
			}
			else
			{
				throw new Exception();
			}
		}

		public async Task<string> GetFileAsync(string fileKey)
		{
			try
			{
				var filePath = Path.Combine(_storagePath, fileKey);
				var fileData = await File.ReadAllTextAsync(filePath);

				if (fileData.Length == 0)
				{
					throw new Exception();
				}

				return fileData;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex);
			}
		}
	}
}
