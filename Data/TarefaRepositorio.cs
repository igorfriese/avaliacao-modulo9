using avaliacao_modulo9.Models;
using Microsoft.EntityFrameworkCore;

namespace avaliacao_modulo9.Data
{
    public class TarefaRepositorio : ITarefaRepositorio
    {
        private readonly AppDbContext _context;

        public TarefaRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tarefa>> ObterTodasAsync()
        {
            return await _context.Tarefas.ToListAsync();
        }

        public async Task<Tarefa> ObterPorIdAsync(int id)
        {
            return await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tarefa> CriarAsync(Tarefa tarefa)
        {
            _context.Tarefas.Add(tarefa);

            await _context.SaveChangesAsync();

            return tarefa;
        }
      
        public async Task<Tarefa> AtualizarAsync(Tarefa tarefa)
        {
            _context.Tarefas.Update(tarefa);

            await _context.SaveChangesAsync();

            return tarefa;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var tarefa = await ObterPorIdAsync(id);

            if (tarefa == null)
                return false;

            _context.Tarefas.Remove(tarefa);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}