namespace ShareBook.Shared.Abstractions.Commands;

public interface ICommandHandler<in TCommand>
	where TCommand : ICommand
{
	Task Handle(TCommand command);
}