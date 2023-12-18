using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace genesis_valida_importacao_teste.Exceptions
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is BadRequestException badRequestException)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    StatusCode = 400,
                    Message = badRequestException.Message
                });
                context.ExceptionHandled = true;
            }

            if (context.Exception is UnprocessableEntityException unprocessableEntityException)
            {
                context.Result = new CustomExceptionResponse(new
                {
                    status = 422,
                    Message = unprocessableEntityException.Message
                });
                context.ExceptionHandled = true;
            }

            if(!context.ExceptionHandled)
            {
                context.Result = new CustomExceptionResponse(new
                {
                    status = 500,
                    Message = context.Exception.Message
                });
                context.ExceptionHandled = true;
            }
        }
    }
}
