using AutoMapper;
using ShareBook.Application.Common;
using ShareBook.Application.Posts.Common;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Posts.Queries.Details;
public class GetPostDetailsRequest : EntityCommand<Guid>, IRequest<Result<PostDetailsOutputModel>>
{
	public class GetPostDetailsHandler : IRequestHandler<GetPostDetailsRequest, Result<PostDetailsOutputModel>>
	{
		private readonly IPostService _postService;
		private readonly IMapper _mapper;

		public GetPostDetailsHandler(
			IPostService postService,
			IMapper mapper)
		{
			_postService = postService;
			_mapper = mapper;
		}

		public async Task<Result<PostDetailsOutputModel>> Handle(GetPostDetailsRequest request, CancellationToken cancellationToken = default)
		{
			var post = await _postService.Details(request.Id);
			return post is null
				? "Requested post does not exist. "
				: _mapper.Map<PostDetailsOutputModel>(post);
		}
	}

}