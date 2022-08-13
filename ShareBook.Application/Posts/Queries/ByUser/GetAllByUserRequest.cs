using AutoMapper;
using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Application.Posts.Common;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Posts.Queries.Mine;
public class GetAllByUserRequest : EntityCommand<Guid>, IRequest<Result<PostsOutputModel>>
{
	public class GetAllByUserHandler : IRequestHandler<GetAllByUserRequest, Result<PostsOutputModel>>
	{
		private readonly IPostService _postService;
		private readonly IMapper _mapper;

		public GetAllByUserHandler(
			IPostService postService,
			IMapper mapper)
		{
			_postService = postService;
			_mapper = mapper;
		}
		public async Task<Result<PostsOutputModel>> Handle(GetAllByUserRequest request, CancellationToken cancellationToken = default)
		{
			var posts = await _postService.GetAllByUserId(request.Id);
			return new PostsOutputModel
			{
				Count = posts.Count(),
				Posts = _mapper.Map<IEnumerable<PostOutputModel>>(posts)
			};
		}
	}
}