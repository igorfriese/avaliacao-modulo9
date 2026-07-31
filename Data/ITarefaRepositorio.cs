using avaliacao_modulo9.Models;

namespace avaliacao_modulo9.Data
{
  
    public interface ITarefaRepositorio
    {
        Task<IEnumerable<Tarefa>> ObterTodasAsync();

        Task<Tarefa> ObterPorIdAsync(int id);

        Task<Tarefa> CriarAsync(Tarefa tarefa);

        Task<Tarefa> AtualizarAsync(Tarefa tarefa);

        Task<bool> DeletarAsync(int id);
    }
}