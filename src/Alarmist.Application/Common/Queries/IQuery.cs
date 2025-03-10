using MediatR;

namespace Alarmist.Application.Common.Queries;

public interface IQuery : IRequest<Result>;

public interface IQuery<TResult> : IRequest<TResult>;