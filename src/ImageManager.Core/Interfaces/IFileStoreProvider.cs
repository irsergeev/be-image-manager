namespace ImageManager.Core.Interfaces
{
	public interface IFileStoreProvider
	{
		Task<string> GetFileAsync(string fileKey);
		Task<string> CreateFileAsync(string fileData);
		Task DeleteFileAsync(string fileKey);
	}
}
