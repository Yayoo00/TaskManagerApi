using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.DTOs;
using TaskManagerApi.Services;

namespace TaskManagerApi.Controllers;

[ApiController]
[Route("api/ml")]
public class MlController : ControllerBase
{
    private readonly SentimientoService _service;

    public MlController(SentimientoService service)
    {
        _service = service;
    }

    [HttpPost("sentimiento")]
    public ActionResult<SentimientoResponse> Analizar(
        SentimientoRequest request)
    {
        var resultado = _service.Predict(request.Comentario);

        return Ok(new SentimientoResponse
        {
            Comentario = request.Comentario,
            Sentimiento = resultado
                ? "Positivo"
                : "Negativo"
        });
    }
}