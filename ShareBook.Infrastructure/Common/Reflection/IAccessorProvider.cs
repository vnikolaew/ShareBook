using System.Linq.Expressions;

namespace ShareBook.Infrastructure.Common.Reflection;
public interface IAccessorProvider
{
	Accessor<TSource, TDest> Get<TSource, TDest>(Expression<Func<TSource, TDest>> selector);
}