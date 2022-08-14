using System.Linq.Expressions;
using System.Reflection;

namespace ShareBook.Infrastructure.Common.Reflection;

public static class Extensions
{
	public static PropertyInfo GetPropertyInfo<S, T>(
		this Expression<Func<S, T>> propertySelector)
	{
		if (propertySelector.Body is not MemberExpression body)
			throw new MissingMemberException("something went wrong");
			
		return body.Member as PropertyInfo;
	}	
}