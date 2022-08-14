using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareBook.Application.Common;
using ShareBook.Application.Profile.Commands.Edit;
using ShareBook.Application.Profile.Commands.EditPhoto;
using ShareBook.Application.Profile.Queries.ByUser;
using ShareBook.Application.Profile.Queries.Mine;
using ShareBook.Web.Common;
using ShareBook.Web.Extensions;

namespace ShareBook.Web.Features;

[Authorize]
public class ProfileController : ApiController
{
	private const long ImageSizeLimit = 10 * 1024 * 1024;
	private const string PhotoEndpoint = "photo";

	[HttpPut]
	public Task<IActionResult> Edit(EditProfileRequest request)
		=> RequestDispatcher
		   .Dispatch<EditProfileRequest, Result>(request)
		   .AcceptedOrBadRequest();

	[HttpPut]
	[Route(PhotoEndpoint)]
	[RequestSizeLimit(ImageSizeLimit)]
	public Task<IActionResult> EditProfilePhoto([FromForm] IFormFile image)
		=> RequestDispatcher
		   .Dispatch<EditProfilePhotoRequest, Result>(image.ToRequest())
		   .AcceptedOrBadRequest();

	[HttpGet]
	public Task<IActionResult> Mine()
		=> SendAsync<GetMyProfileRequest, ProfileOutputModel>(new GetMyProfileRequest());

	[HttpGet]
	[Route(GuidId)]
	public Task<IActionResult> ByUser(Guid id)
		=> RequestDispatcher
		   .Dispatch<GetUserProfileRequest, Result<ProfileOutputModel>>(new GetUserProfileRequest().WithId(id))
		   .OkOrNotFound();	
}