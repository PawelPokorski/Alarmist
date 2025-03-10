using MediatR;

namespace Alarmist.Application.Common.Commands;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResult> : IRequest<TResult>;