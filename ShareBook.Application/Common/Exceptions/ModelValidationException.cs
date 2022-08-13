using FluentValidation.Results;

namespace ShareBook.Application.Common.Exceptions;

public class ModelValidationException : Exception
{
	public ModelValidationException()
		: base("One or more validation exceptions have occured")
		=> Errors = new Dictionary<string, string[]>();

	public IDictionary<string, string[]> Errors { get; set; }

	public ModelValidationException(IEnumerable<ValidationFailure> failures)
		: this()
	{
		foreach (var source in failures.GroupBy(fg => fg.PropertyName, fg => fg.ErrorMessage))
		{
			Errors.Add(source.Key, source.ToArray());
		}
	}
}