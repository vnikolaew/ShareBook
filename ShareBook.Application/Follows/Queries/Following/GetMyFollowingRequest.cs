using AutoMapper;
using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Application.Follows.Queries.Common;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Follows.Queries.Following;
public class GetMyFollowingRequest : IRequest<Result<FollowingOutputModel>>
{
	public class GetMyFollowingHandler : IRequestHandler<GetMyFollowingRequest, Result<FollowingOutputModel>>
	{
		private readonly IFollowService _follows;
		private readonly ICurrentUser _currentUser;
		private readonly IMapper _mapper;

		public GetMyFollowingHandler(
			IFollowService follows,
			ICurrentUser currentUser,
			IMapper mapper)
		{
			_follows = follows;
			_currentUser = currentUser;
			_mapper = mapper;
		}

		public async Task<Result<FollowingOutputModel>> Handle(GetMyFollowingRequest request, CancellationToken cancellationToken = default)
		{
			var follows = await _follows.GetFollowing(_currentUser.UserId);
			return Result<FollowingOutputModel>.SuccessWith(new FollowingOutputModel
			{
				Count = follows.Count(),
				Following = _mapper.Map<IEnumerable<FolloweeOutputModel>>(follows)
			});
		}
	}

}