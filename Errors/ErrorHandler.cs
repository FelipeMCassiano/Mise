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
                CreateProductError(error),
            ErrorType.NotFoundProduct =>
            NotFoundError(error),
            ErrorType.NotFoundTag =>
               NotFoundError(error),
            ErrorType.AlreadyExistingTags =>
            AlreadyExistsTagError(error),
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
        return new ObjectResult(new
        {
            status = "error",
            message = error.Message,
            errorType = error.ErrorType.ToString()

        })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
    private IActionResult AlreadyExistsTagError(Error error)
    {
        return new ObjectResult(new
        {
            status = "error",
            message = error.Message,
            errorType = error.ErrorType.ToString()

        })
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }

}