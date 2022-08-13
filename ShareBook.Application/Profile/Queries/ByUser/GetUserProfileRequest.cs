using AutoMapper;
using ShareBook.Application.Common;
using ShareBook.Application.Profile.Queries.Mine;
using ShareBook.Domain.Models.Profile.Repositories;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Profile.Queries.ByUser;
public class GetUserProfileRequest : EntityCommand<Guid>, IRequest<Result<ProfileOutputModel>>
{
	public class GetUserProfileHandler : IRequestHandler<GetUserProfileRequest, Result<ProfileOutputModel>>
	{
		private readonly IProfileRepository _profiles;
		private readonly IMapper _mapper;

		public GetUserProfileHandler(
			IProfileRepository profiles,
			IMapper mapper)
		{
			_profiles = profiles;
			_mapper = mapper;
		}

		public async Task<Result<ProfileOutputModel>> Handle(
			GetUserProfileRequest request,
			CancellationToken cancellationToken = default)
		{
			var profile = await _profiles.GetByUserId(request.Id, cancellationToken);
			if (profile is null)
			{
				return Result<ProfileOutputModel>.Failure(new []{"User does not exist. "});
			}
			return _mapper.Map<ProfileOutputModel>(profile);
		}
	}
}