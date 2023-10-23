using AutoMapper;
using ImageManager.Core.Models;
using ImageManager.NHibernate.Data;

namespace ImageManager.Core.Mappers
{
	public class ImageMapper : Profile
	{
		public ImageMapper()
		{
			CreateMap<ImageDto, ImageModel>();
			CreateMap<ImageModel, ImageDto>();
		}
	}
}
