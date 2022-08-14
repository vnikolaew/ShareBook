using AutoMapper;
using ShareBook.Application.Common;
using ShareBook.Application.Likes.Queries.Common;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Likes.Queries.AllByPost;
public class GetAllByPostRequest : EntityCommand<Guid>, IRequest<Result<LikesOutputModel>>
{
	public class GetAllByPostHandler : IRequestHandler<GetAllByPostRequest, Result<LikesOutputModel>>
	{
		private readonly ILikesService _likes;
		private readonly IMapper _mapper;

		public GetAllByPostHandler(
			ILikesService likes,
			IMapper mapper)
		{
			_likes = likes;
			_mapper = mapper;
		}

		public async Task<Result<LikesOutputModel>> Handle(
			GetAllByPostRequest request,
			CancellationToken cancellationToken = default)
		{
			var likes = await _likes.GetAllByPostId(request.Id, cancellationToken);
			return likes is null
				? "Requested post does not exist."
				: new LikesOutputModel
				{
					Likes = _mapper.Map<IEnumerable<LikeOutputModel>>(likes)
				};
		}
	}
}