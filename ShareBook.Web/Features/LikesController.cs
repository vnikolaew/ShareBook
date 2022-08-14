using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareBook.Application.Common;
using ShareBook.Application.Likes.Commands.Like;
using ShareBook.Application.Likes.Queries.AllByPost;
using ShareBook.Application.Likes.Queries.Common;
using ShareBook.Web.Common;
using ShareBook.Web.Extensions;

namespace ShareBook.Web.Features;

[Authorize]
public class LikesController : ApiController
{
	[HttpPost]
	[Route(GuidId)]
	public Task<IActionResult> Like(Guid id)
		=> RequestDispatcher
		   .Dispatch<LikePostRequest, Result>(new LikePostRequest().WithId(id))
		   .AcceptedOrBadRequest();

	[HttpDelete]
	[Route(GuidId)]
	public Task<IActionResult> Unlike(Guid id)
		=> RequestDispatcher
		   .Dispatch<UnlikePostRequest, Result>(new UnlikePostRequest().WithId(id))
		   .AcceptedOrBadRequest();

	[HttpGet]
	[Route(GuidId)]
	public Task<IActionResult> ByPost(Guid id)
		=> RequestDispatcher
		   .Dispatch<GetAllByPostRequest, Result<LikesOutputModel>>(new GetAllByPostRequest().WithId(id))
		   .OkOrNotFound();	
}