using genesis_valida_importacao_teste.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
           
            _consumidor.IniciaConsumoParaleloSyncTalvez();
            return Ok();
        }

    }
}

