namespace Alarmist.Application.Common;

public record Result
{
    public bool IsSuccess { get; init; }
    public string[] Errors { get; init; } = Array.Empty<string>();

    public static Result Success() => new() { IsSuccess = true };
    public static Result Failure(params string[] errors) => new() { IsSuccess = false, Errors = errors };
} 