using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareBook.Application.Common;
using ShareBook.Application.Posts.Commands.Create;
using ShareBook.Application.Posts.Commands.Delete;
using ShareBook.Application.Posts.Commands.Edit;
using ShareBook.Application.Posts.Common;
using ShareBook.Application.Posts.Queries.Details;
using ShareBook.Application.Posts.Queries.Mine;
using ShareBook.Web.Common;
using ShareBook.Web.Extensions;

namespace ShareBook.Web.Features;

[Authorize]
public class PostsController : ApiController
{
	private const string ByUserEndpoint = "user";
	private const long ImageSizeLimit = 10 * 1024 * 1024;

	[HttpPost]
	[RequestSizeLimit(ImageSizeLimit)]
	public Task<IActionResult> Create([FromForm] IFormFile image, [FromForm] string content)
		=> RequestDispatcher
		   .Dispatch<CreatePostRequest, Result>(new CreatePostRequest
			{
				Content = content,
				Media = image.ToPostMediaModel(),
			})
		   .AcceptedOrBadRequest();

	[HttpGet]
	[Route(nameof(Mine))]
	public Task<IActionResult> Mine()
		=> SendAsync<GetMineRequest, PostsOutputModel>(new GetMineRequest());

	[HttpGet]
	[Route($"{ByUserEndpoint}/{GuidId}")]
	public Task<IActionResult> ByUser(Guid id)
		=> RequestDispatcher
			.Dispatch<GetAllByUserRequest, Result<PostsOutputModel>>(new GetAllByUserRequest().WithId(id))
			   .OkOrNotFound();

	[HttpGet]
	[Route(GuidId)]
	public Task<IActionResult> Details(Guid id)
		=> RequestDispatcher
			.Dispatch<GetPostDetailsRequest, Result<PostDetailsOutputModel>>(new GetPostDetailsRequest().WithId(id))
			.OkOrNotFound();

	[HttpPut]
	[Route(GuidId)]
	public Task<IActionResult> Edit(EditPostRequest request, Guid id)
		=> RequestDispatcher
		   .Dispatch<EditPostRequest, Result>(request.WithId(id))
		   .AcceptedOrBadRequest();

	[HttpDelete]
	[Route(GuidId)]
	public Task<IActionResult> Delete(Guid id)
		=> SendAsync(new DeletePostRequest().WithId(id));	
}