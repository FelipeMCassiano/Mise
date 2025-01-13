namespace Mise.Errors;

public record AlreadyExistingTagsError() : Error("These tags Already exist", ErrorType.AlreadyExistingTags);