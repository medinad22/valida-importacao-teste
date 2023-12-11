using genesis_valida_importacao_teste.consome_smtp;
using genesis_valida_importacao_teste.Interfaces;
using genesis_valida_importacao_teste.valida_arquivo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Linq;

namespace genesis_valida_importacao_teste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SincronizaProcesso(IConsumidor consumidor) : ControllerBase
    {
        private readonly IConsumidor _consumidor = consumidor;

        [HttpGet]
        public IActionResult Processo()
        {
           
            _consumidor.IniciaConsumoParalelo();
            return Ok();
        }

    }
}

