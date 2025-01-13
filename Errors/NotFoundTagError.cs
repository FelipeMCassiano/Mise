namespace Mise.Errors;
public record NotFoundTagError() : Error("Tag not found", ErrorType.NotFoundTag);