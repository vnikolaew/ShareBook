namespace ShareBook.Application.Common;

public static class EntityCommandExtensions
{
	public static TCommand WithId<TCommand, TId>(this TCommand command, TId id)
		where TCommand : EntityCommand<TId>
	{
		command.Id = id;
		return command;
	}
}