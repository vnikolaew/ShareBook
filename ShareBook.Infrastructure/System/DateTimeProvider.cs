using ShareBook.Application.Common.Contracts;

namespace ShareBook.Infrastructure.System;

public class DateTimeProvider : IDateTime
{
	public DateTime Now => DateTime.UtcNow;
}