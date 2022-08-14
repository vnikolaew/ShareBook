namespace ShareBook.Infrastructure.Common.Persistence.Mappings;

public interface INodeMapping<in T> where T : class
{
	object Map(T entity);
}