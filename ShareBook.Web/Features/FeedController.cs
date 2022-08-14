using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareBook.Application.Feed;
using ShareBook.Application.Feed.Queries.Mine;
using ShareBook.Web.Common;

namespace ShareBook.Web.Features;

[Authorize]
public class FeedController : ApiController
{
	[HttpGet]
	public Task<IActionResult> Generate()
		=> SendAsync<GetMyFeedRequest, FeedOutputModel>(new GetMyFeedRequest());	
}