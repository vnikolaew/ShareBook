using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace ShareBook.Infrastructure.Common.Reflection;

public class AccessorProvider : IAccessorProvider
{
	private readonly ConcurrentDictionary<(Type, Expression), object>
		AccessorCache = new();

	public Accessor<TSource, TDest> Get<TSource, TDest>(Expression<Func<TSource, TDest>> selector)
		=> (Accessor<TSource, TDest>) AccessorCache.GetOrAdd((typeof (TSource), selector.Body),
			(_, _) => Accessor<TSource>.Create(selector), (Accessor<TSource, TDest>) null);
}