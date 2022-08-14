using AutoMapper;
using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Application.Follows.Queries.Common;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Follows.Queries.Followers;

public class GetMyFollowersRequest : IRequest<Result<FollowersOutputModel>>
{
	public class GetMyFollowersHandler : IRequestHandler<GetMyFollowersRequest, Result<FollowersOutputModel>>
	{
		private readonly IFollowService _follows;
		private readonly ICurrentUser _currentUser;
		private readonly IMapper _mapper;

		public GetMyFollowersHandler(
			IFollowService follows,
			ICurrentUser currentUser,
			IMapper mapper)
		{
			_follows = follows;
			_currentUser = currentUser;
			_mapper = mapper;
		}

		public async Task<Result<FollowersOutputModel>> Handle(
			GetMyFollowersRequest request,
			CancellationToken cancellationToken = default)
		{
			var follows = await _follows.GetFollowers(_currentUser.UserId);
			return new FollowersOutputModel
			{
				Count = follows.Count(),
				Followers = _mapper.Map<IEnumerable<FollowerOutputModel>>(follows)
			};
		}
	}

}