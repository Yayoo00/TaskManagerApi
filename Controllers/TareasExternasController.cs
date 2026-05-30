using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers;

[ApiController]
[Route("api/tareas-externas")]
public class TareasExternasController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public TareasExternasController(
        IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TareaExternaDto>>> Get()
    {
        try
        {
            var response =
                await _httpClient.GetFromJsonAsync<List<TodoApiResponse>>(
                    "https://jsonplaceholder.typicode.com/todos");

            var resultado = response!.Select(x =>
                new TareaExternaDto
                {
                    ExternalId = x.Id,
                    Titulo = x.Title,
                    Completado = x.Completed
                });

            return Ok(resultado);
        }
        catch
        {
            return StatusCode(
                503,
                "No se pudo acceder a la API externa");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TareaExternaDto>> GetById(int id)
    {
        try
        {
            var todo =
                await _httpClient.GetFromJsonAsync<TodoApiResponse>(
                    $"https://jsonplaceholder.typicode.com/todos/{id}");

            if (todo == null)
                return NotFound();

            return Ok(new TareaExternaDto
            {
                ExternalId = todo.Id,
                Titulo = todo.Title,
                Completado = todo.Completed
            });
        }
        catch
        {
            return NotFound();
        }
    }
}