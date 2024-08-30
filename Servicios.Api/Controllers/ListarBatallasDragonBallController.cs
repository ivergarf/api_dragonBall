using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Servicios.Api.Responses;
using Servicios.Core.DTOs;
using Servicios.Core.Exceptions;
using Servicios.Core.Interfaces;
using Servicios.Core.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics;


namespace Servicios.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ListarBatallasDragonBallController : ControllerBase
    {
        private readonly IListarBatallasDragonBallService _ListarBatallasDragonBallService;
        private readonly ILogger _logger;

        public ListarBatallasDragonBallController(IListarBatallasDragonBallService listarBatallasDragonBallService, ILogger<ListarBatallasDragonBallController> logger)
        {
            _ListarBatallasDragonBallService = listarBatallasDragonBallService;
            _logger = logger;
        }

        [SwaggerOperation(Summary = "Consulta que se realiza para listar batallas de dragon ball")]
        [HttpGet("~/api/v1/ProgramarBatallas")]
        public async Task<IActionResult> ProgramarBatallas([FromQuery] int limit)

        {
            var jsonEntrada = JsonConvert.SerializeObject(limit);
            object response;
            Stopwatch time = Stopwatch.StartNew();
            try
            {
                var dragonballResponse = await _ListarBatallasDragonBallService.ProgramarBatallas(limit);
                time.Stop();
                if (dragonballResponse.response == "OK")
                {

                    response = new ApiResponse<PersonajeDragonBallresponseDto>(dragonballResponse, $"{time.ElapsedMilliseconds} ms", true);
                }
                else
                {
                    response = new ApiResponse<PersonajeDragonBallresponseDto>(dragonballResponse, $"{time.ElapsedMilliseconds} ms", false);
                }

                var jsonSalida = JsonConvert.SerializeObject(response);
                _logger.LogInformation("API: /api/v1/dragonball/ Entrada: {Entrada}, Respuesta: {Respuesta}, Tiempo: {Tiempo} ms", jsonEntrada, jsonSalida, time.ElapsedMilliseconds);

                return Ok(response);

            }
            catch (Exception ex)
            {
                time.Stop();
                _logger.LogError("API: /api/v1/dragonball/ Entrada: {Entrada}, Respuesta: {Respuesta}, Tiempo: {Tiempo} ms", jsonEntrada, ex.Message, time.ElapsedMilliseconds);
                throw new BusinessException(ex.Message.ToString());
            }

        }
    }
}
