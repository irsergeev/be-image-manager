using AutoMapper;
using ImageManager.Core.Models;
using ImageManager.NHibernate.Data;

namespace ImageManager.Core.Mappers
{
	public class CommentMapper : Profile
	{
		public CommentMapper()
		{
			CreateMap<CommentDto, CommentModel>();
			CreateMap<CommentModel, CommentDto>();
		}
	}
}
