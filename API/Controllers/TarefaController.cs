using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using System.Collections.Generic;
using System.Linq;


namespace API.Controllers;

[Route("api/tarefa")]
[ApiController]
public class TarefaController : ControllerBase
{
    private readonly AppDataContext _context;

    public TarefaController(AppDataContext context) =>
        _context = context;

    // GET: api/tarefa/listar
    [HttpGet]
    [Route("listar")]
    public IActionResult Listar()
    {
        try
        {
            List<Tarefa> tarefas = _context.Tarefas.Include(x => x.Categoria).ToList();
            return Ok(tarefas);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // POST: api/tarefa/cadastrar
    [HttpPost]
    [Route("cadastrar")]
    public IActionResult Cadastrar([FromBody] Tarefa tarefa)
    {
        try
        {
            Categoria? categoria = _context.Categorias.Find(tarefa.CategoriaId);
            if (categoria == null)
            {
                return NotFound();
            }
            tarefa.Categoria = categoria;
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
            return Created("", tarefa);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch]
    [Route("alterar/{id}")]
    public IActionResult Alterar([FromRoute] int id, [FromBody] Tarefa tarefa)
    {
        try
        {
            Tarefa? tarefaCadastrada =
                _context.Tarefas.FirstOrDefault(x => x.TarefaId == id);
            if (tarefaCadastrada != null)
            {
                if (tarefaCadastrada.Status != "Aberto")
                {
                    return BadRequest("A tarefa não pode ser alterada.");
                }

                string statusOriginal = tarefaCadastrada.Status;

                Categoria? categoria =
                    _context.Categorias.Find(tarefa.CategoriaId);
                if (categoria == null)
                {
                    return NotFound();
                }

                tarefaCadastrada.Categoria = categoria;
                tarefaCadastrada.Titulo = tarefa.Titulo;
                tarefaCadastrada.Descricao = tarefa.Descricao;
                tarefaCadastrada.Status = "Em andamento";

                _context.Tarefas.Update(tarefaCadastrada);
                _context.SaveChanges();

                return Ok();
            }
            return NotFound();
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("tarefasEspecificas")]
    public IActionResult Tarefas([FromBody] Tarefa tarefa)
    {
        try
        {
            List<Tarefa> tarefas = _context.Tarefas.Where
                (x => x.Status == "Não iniciada" || x.Status == "Em andamento").ToList();

            return Ok(tarefas);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("tarefasConcluidas")]
    public IActionResult TarefasConcluidas([FromBody] Tarefa tarefa)
    {
        try
        {
            List<Tarefa> tarefas = _context.Tarefas.Where(x => x.Status == "Concluida").ToList();

            return Ok(tarefas);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}



