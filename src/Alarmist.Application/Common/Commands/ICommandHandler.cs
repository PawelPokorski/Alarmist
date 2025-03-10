using MediatR;

namespace Alarmist.Application.Common.Commands;

// Domyślny handler dla ICommand (automatycznie używa Result)
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand;

// Handler dla niestandardowego typu zwracanego
public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>;