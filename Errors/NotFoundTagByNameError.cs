namespace Mise.Errors;

public record NotFoundTagByNameError(string TagName) : Error("Tag not found", ErrorType.NotFoundTag);
