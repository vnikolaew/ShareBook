using AutoMapper;
using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Application.Posts.Common;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Posts.Queries.Mine;
public class GetMineRequest : IRequest<Result<PostsOutputModel>>
{
	public class GetMineHandler : IRequestHandler<GetMineRequest, Result<PostsOutputModel>>
	{
		private readonly ICurrentUser _currentUser;
		private readonly IPostService _postService;
		private readonly IMapper _mapper;

		public GetMineHandler(
			ICurrentUser currentUser,
			IPostService postService,
			IMapper mapper)
		{
			_currentUser = currentUser;
			_postService = postService;
			_mapper = mapper;
		}

		public async Task<Result<PostsOutputModel>> Handle(GetMineRequest request, CancellationToken cancellationToken = default)
		{
			var posts = await _postService.GetAllByUserId(_currentUser.UserId);
			return new PostsOutputModel
			{
				Count = posts.Count(),
				Posts = _mapper.Map<IEnumerable<PostOutputModel>>(posts)
			};
		}
	}
}