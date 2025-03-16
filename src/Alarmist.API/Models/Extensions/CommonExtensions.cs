using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Alarmist.API.Models.Extensions;

public static class CommonExtensions
{
    public static string GetFirstError(this ModelStateDictionary modelState)
    {
        return modelState.Values
            .SelectMany(v => v.Errors)
            .FirstOrDefault()?.ErrorMessage;
    }
}
