using ShareBook.Shared.Abstractions.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace ShareBook.Shared;

public class InMemoryRequestDispatcher : IRequestDispatcher
{
  private readonly IServiceScopeFactory _scopeFactory;

  public InMemoryRequestDispatcher(IServiceScopeFactory scopeFactory)
   => _scopeFactory = scopeFactory;

  public async Task Dispatch<TRequest>(TRequest request, CancellationToken cancellationToken = default)
      where TRequest : IRequest
  {
    await using var scope = _scopeFactory.CreateAsyncScope();
    
    var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest>>();
    
    await handler.Handle(request, cancellationToken);
  }

  public async Task<TResult> Dispatch<TRequest, TResult>(
    TRequest request,
    CancellationToken cancellationToken = default)
      where TRequest : IRequest<TResult>
  {
    await using var scope = _scopeFactory.CreateAsyncScope();
    var handler = scope.ServiceProvider
                       .GetRequiredService<IRequestHandler<TRequest, TResult>>();
    
    return await handler.Handle(request, cancellationToken);
  }
}