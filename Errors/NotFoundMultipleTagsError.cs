namespace Mise.Errors;

public record NotFoundMultipleTagsError(List<string> TagNames) : Error("Multiple Tags not found", ErrorType.NotFoundTag);