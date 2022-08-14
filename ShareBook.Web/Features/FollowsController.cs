using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareBook.Application.Common;
using ShareBook.Application.Follows.Commands.Follow;
using ShareBook.Application.Follows.Queries.Common;
using ShareBook.Application.Follows.Queries.Followers;
using ShareBook.Application.Follows.Queries.Following;
using ShareBook.Web.Common;

namespace ShareBook.Web.Features;

[Authorize]
public class FollowsController : ApiController
{
	private const string FollowersEndpoint = "/followers";
	private const string FollowingEndpoint = "/following";

	[HttpPost]
	[Route(GuidId)]
	public Task<IActionResult> Follow(Guid id)
		=> SendAsync(new FollowUserRequest().WithId(id));

	[HttpDelete]
	[Route(GuidId)]
	public Task<IActionResult> Unfollow(Guid id)
		=> SendAsync(new UnfollowUserRequest().WithId(id));

	[HttpGet]
	[Route(FollowersEndpoint)]
	public Task<IActionResult> Followers()
		=> SendAsync<GetMyFollowersRequest, FollowersOutputModel>(new GetMyFollowersRequest());

	[HttpGet]
	[Route(FollowingEndpoint)]
	public Task<IActionResult> Following()
		=> SendAsync<GetMyFollowingRequest, FollowingOutputModel>(new GetMyFollowingRequest());	
}