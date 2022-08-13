using Microsoft.Extensions.Logging;
using ShareBook.Application.Common.Contracts;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Common.Behaviours;
public class LoggingHandlerDecorator<TRequest, TResult>
  : IRequestHandler<TRequest, TResult>
  where TRequest : IRequest<TResult>
{
  private readonly IRequestHandler<TRequest, TResult> _inner;
  private readonly ICurrentUser _currentUser;
  private readonly ILogger<LoggingHandlerDecorator<TRequest, TResult>> _logger;

  public LoggingHandlerDecorator(
    IRequestHandler<TRequest, TResult> inner,
    ILogger<LoggingHandlerDecorator<TRequest, TResult>> logger,
    ICurrentUser currentUser)
  {
    _inner = inner;
    _logger = logger;
    _currentUser = currentUser;
  }

  public Task<TResult> Handle(TRequest request, CancellationToken cancellationToken = default (CancellationToken))
  {
    _logger.LogInformation("Incoming request: {Request} by user {@UserName}",
      typeof (TRequest).Name, _currentUser.UserName ?? string.Empty);
    return _inner.Handle(request, cancellationToken);
  }
}