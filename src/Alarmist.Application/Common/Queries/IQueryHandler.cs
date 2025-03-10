using MediatR;

namespace Alarmist.Application.Common.Queries;

public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>;