using AutoMapper;
using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Feed.Queries.Mine;
public class GetMyFeedRequest : IRequest<Result<FeedOutputModel>>
{
	public class GetMyFeedHandler : IRequestHandler<GetMyFeedRequest, Result<FeedOutputModel>>
	{
		private readonly IFeedService _feed;
		private readonly ICurrentUser _currentUser;
		private readonly IMapper _mapper;

		public GetMyFeedHandler(
			IFeedService feed,
			ICurrentUser currentUser,
			IMapper mapper)
		{
			_feed = feed;
			_currentUser = currentUser;
			_mapper = mapper;
		}

		public async Task<Result<FeedOutputModel>> Handle(GetMyFeedRequest request, CancellationToken cancellationToken = default)
		{
			var feed = await _feed.Generate(_currentUser.UserId, cancellationToken);
			return new FeedOutputModel
			{
				Count = feed.Count(),
				Posts = _mapper.Map<IEnumerable<FeedPostOutputModel>>(feed)
			};
		}
	}

}