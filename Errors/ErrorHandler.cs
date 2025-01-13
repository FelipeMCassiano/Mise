using Microsoft.AspNetCore.Mvc;

namespace Mise.Errors;

public interface IErrorHandler
{
    IActionResult Handle(Error error);
}

public class ErrorHandler : IErrorHandler
{
    public IActionResult Handle(Error error)
    {
        IActionResult actionResult = error.ErrorType switch
        {
            ErrorType.CreateProductError =>
                ServerError(error),
            ErrorType.NotFoundProduct =>
            NotFoundError(error),
            ErrorType.NotFoundTag =>
               NotFoundError(error),
            _ =>
                 ServerError(error)


        };
        return actionResult;
    }

    private IActionResult ServerError(Error error)
    {
        return new ObjectResult(
        new
        {
            status = "error",
            message = error.Message,
            errorType = error.ErrorType.ToString()
        })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }

    private IActionResult NotFoundError(Error error)
    {

        return new ObjectResult(new
        {
            status = "error",
            message = error.Message,
            errorType = error.ErrorType.ToString()

        })
        {
            StatusCode = StatusCodes.Status404NotFound
        };


    }


    private IActionResult CreateProductError(Error error)
    {
        if (error.ErrorType == ErrorType.CreateProductError) {; }
        throw new NotImplementedException();
    }

}