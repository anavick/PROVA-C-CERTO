namespace API.Models;

public class Tarefa
{
    public int TarefaId { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.Now;
    public Categoria? Categoria { get; set; }
    public int CategoriaId { get; set; }
    public string? Status { get; set;}
}
