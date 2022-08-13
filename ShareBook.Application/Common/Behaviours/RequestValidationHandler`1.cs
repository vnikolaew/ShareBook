using FluentValidation;
using FluentValidation.Results;
using ShareBook.Application.Common.Exceptions;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Common.Behaviours;

public class RequestValidationHandler<TRequest>
  : IRequestHandler<TRequest>
  where TRequest : IRequest
{
  private readonly IEnumerable<IValidator<TRequest>> _validators;
  private readonly IRequestHandler<TRequest> _inner;

  public RequestValidationHandler(
    IEnumerable<IValidator<TRequest>> validators,
    IRequestHandler<TRequest> inner)
  {
    _validators = validators;
    _inner = inner;
  }

  public async Task Handle(TRequest command, CancellationToken cancellationToken)
  {
    var context = new ValidationContext<TRequest>(command);
    
    var list = _validators
               .Select(v => v.Validate(context))
               .SelectMany(r => r.Errors)
               .Where<ValidationFailure>(f => f != null)
               .ToList();

    if (list.Any())
    {
      throw new ModelValidationException((IEnumerable<ValidationFailure>) list);
    }
    
    await _inner.Handle(command, cancellationToken);
  }
}

public class RequestValidationHandler<TRequest, TResult>
  : IRequestHandler<TRequest, TResult>
  where TRequest : IRequest<TResult>
{
  private readonly IEnumerable<IValidator<TRequest>> _validators;
  private readonly IRequestHandler<TRequest, TResult> _inner;

  public RequestValidationHandler(
    IEnumerable<IValidator<TRequest>> validators,
    IRequestHandler<TRequest, TResult> inner)
  {
    _validators = validators;
    _inner = inner;
  }

  public async Task<TResult> Handle(TRequest query,
    CancellationToken cancellationToken = default)
  {
    var context = new ValidationContext<TRequest>(query);
    
    var list = _validators
               .Select(v => v.Validate(context))
               .SelectMany(r => r.Errors)
               .Where<ValidationFailure>(f => f != null)
               .ToList();

    if (list.Any())
    {
      throw new ModelValidationException((IEnumerable<ValidationFailure>) list);
    }
    
    return await _inner.Handle(query, cancellationToken);
  }
}