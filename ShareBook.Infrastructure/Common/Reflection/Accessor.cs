using System.Linq.Expressions;
using System.Reflection;

namespace ShareBook.Infrastructure.Common.Reflection;
public class Accessor<S>
{
	public static Accessor<S, T> Create<T>(Expression<Func<S, T>> memberSelector)
		=> new GetterSetter<T>(memberSelector);

	public Accessor<S, T> Get<T>(Expression<Func<S, T>> memberSelector)
		=> Create(memberSelector);

	private class GetterSetter<T> : Accessor<S, T>
	{
		public GetterSetter(Expression<Func<S, T>> memberSelector)
			: base(memberSelector)
		{
		}
	}	
}

public class Accessor<S, T> : Accessor<S>
{
	private readonly Func<S, T> _getter;
	private readonly Action<S, T> _setter;

	private bool IsReadable { get; set; }

	private bool IsWritable { get; set; }

	public T this[S instance]
	{
		get
		{
			if (!IsReadable)
				throw new ArgumentException("Property get method not found.");
			return _getter(instance);
		}
		set
		{
			if (!IsWritable)
				throw new ArgumentException("Property set method not found.");
			_setter(instance, value);
		}
	}

	public Accessor(Expression<Func<S, T>> memberSelector)
	{
		var propertyInfo = memberSelector.GetPropertyInfo();
		IsReadable = propertyInfo.CanRead;
		IsWritable = propertyInfo.CanWrite;
		
		AssignDelegate(IsReadable, ref _getter, propertyInfo.GetGetMethod(true));
		AssignDelegate(IsWritable, ref _setter, propertyInfo.GetSetMethod(true));
	}

	private static void AssignDelegate<K>(bool assignable, ref K assignee, MethodInfo assignor) where K : class, Delegate
	{
		if (!assignable)
			return;
		assignee = assignor.CreateDelegate<K>();
	}
}