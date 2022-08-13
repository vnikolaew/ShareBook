using AutoMapper;
using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Domain.Models.Profile.Repositories;
using ShareBook.Domain.Models.User;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Profile.Queries.Mine;
public class GetMyProfileRequest : IRequest<Result<ProfileOutputModel>>
{
	public class GetMyProfileHandler : 
		IRequestHandler<GetMyProfileRequest, Result<ProfileOutputModel>>
	{
		private readonly ICurrentUser _currentUser;
		private readonly IProfileRepository _profiles;
		private readonly IMapper _mapper;

		public GetMyProfileHandler(
			ICurrentUser currentUser,
			IProfileRepository profiles,
			IMapper mapper)
		{
			_currentUser = currentUser;
			_profiles = profiles;
			_mapper = mapper;
		}

		public async Task<Result<ProfileOutputModel>> Handle(
			GetMyProfileRequest request,
			CancellationToken cancellationToken)
		{
			var profile = await _profiles.GetByUserId(_currentUser.UserId, cancellationToken);
			return _mapper.Map<ProfileOutputModel>(profile);
		}
	}
}