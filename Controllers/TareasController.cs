using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TareasController : ControllerBase
{
    private readonly AppDbContext _context;

    public TareasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
public async Task<ActionResult<IEnumerable<Tarea>>> GetTareas(
    string? estado,
    string? prioridad,
    DateTime? fechaInicio,
    DateTime? fechaFin)
{
    var query = _context.Tareas.AsQueryable();

    if (fechaInicio.HasValue && fechaFin.HasValue &&
        fechaInicio > fechaFin)
    {
        return BadRequest(
            "fechaInicio no puede ser mayor que fechaFin");
    }

    if (!string.IsNullOrEmpty(estado))
    {
        if (!Enum.TryParse<EstadoTarea>(
            estado,
            true,
            out var estadoEnum))
        {
            return BadRequest("Estado inválido");
        }

        query = query.Where(t =>
            t.Estado == estadoEnum);
    }

    if (!string.IsNullOrEmpty(prioridad))
    {
        if (!Enum.TryParse<PrioridadTarea>(
            prioridad,
            true,
            out var prioridadEnum))
        {
            return BadRequest("Prioridad inválida");
        }

        query = query.Where(t =>
            t.Prioridad == prioridadEnum);
    }

    if (fechaInicio.HasValue)
    {
        query = query.Where(t =>
            t.FechaVencimiento >= fechaInicio.Value);
    }

    if (fechaFin.HasValue)
    {
        query = query.Where(t =>
            t.FechaVencimiento <= fechaFin.Value);
    }

    return await query.ToListAsync();
}

    [HttpGet("{id}")]
    public async Task<ActionResult<Tarea>> GetTarea(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);

        if (tarea == null)
            return NotFound();

        return tarea;
    }

    [HttpPost]
    public async Task<ActionResult<Tarea>> PostTarea(Tarea tarea)
    {
        if (tarea.FechaVencimiento.Date < DateTime.Today)
            return BadRequest("La fecha de vencimiento no puede ser menor a la fecha actual.");

        _context.Tareas.Add(tarea);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTarea),
            new { id = tarea.Id },
            tarea);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTarea(int id, Tarea tarea)
    {
        if (id != tarea.Id)
            return BadRequest();

        if (tarea.FechaVencimiento.Date < DateTime.Today)
            return BadRequest("La fecha de vencimiento no puede ser menor a la fecha actual.");

        _context.Entry(tarea).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTarea(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);

        if (tarea == null)
            return NotFound();

        _context.Tareas.Remove(tarea);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}