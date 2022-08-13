namespace ShareBook.Shared.Abstractions.Commands;

public interface ICommandIDispatcher
{
	Task Dispatch<TCommand>(TCommand command) where TCommand : ICommand;
}