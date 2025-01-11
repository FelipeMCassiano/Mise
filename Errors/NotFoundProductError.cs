namespace Mise.Errors;

public record NotFoundProductError() : Error("Product Not found", ErrorType.NotFoundProduct);