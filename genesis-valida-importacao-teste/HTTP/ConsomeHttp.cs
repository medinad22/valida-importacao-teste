using genesis_valida_importacao_teste.Exceptions;
using genesis_valida_importacao_teste.HTTP.Liberty.Models;
using Microsoft.AspNetCore.Mvc;

namespace genesis_valida_importacao_teste.HTTP
{
    [Route("maxpar/v1/apolices")]
    [ApiController]
    public class ConsomeHttp : ControllerBase
    {

        [HttpPost]
        public IActionResult RecebeApoliceLiberty([FromBody] LibertyModel liberty)
        {

            
            return Ok();
        }
    }
}
