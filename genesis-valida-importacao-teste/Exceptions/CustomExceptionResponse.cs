using Microsoft.AspNetCore.Mvc;

namespace genesis_valida_importacao_teste.Exceptions
{
    public class CustomExceptionResponse : ObjectResult
    {
        public CustomExceptionResponse(object? value) : base(value)
        {
        }
    }
}
