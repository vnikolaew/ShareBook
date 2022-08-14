using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareBook.Application.Comments.Commands.Create;
using ShareBook.Application.Comments.Commands.Edit;
using ShareBook.Application.Common;
using ShareBook.Web.Common;
using ShareBook.Web.Extensions;

namespace ShareBook.Web.Features;

[Authorize]
public class CommentsController : ApiController
{
    [HttpPost]
    [Route(GuidId)]
    public Task<IActionResult> Comment(CreateCommentRequest request, Guid id)
	    => SendAsync(request.WithId(id));

    [HttpPut]
    [Route(GuidId)]
    public Task<IActionResult> Edit(EditCommentRequest request, Guid id)
		 => RequestDispatcher
		    .Dispatch<EditCommentRequest, Result>(request.WithId(id))
		    .AcceptedOrBadRequest();	
}