using Microsoft.AspNetCore.Mvc;
using avaliacao_modulo9.Data;
using avaliacao_modulo9.Models;

namespace avaliacao_modulo9.Controllers
{
    public class TarefasController : Controller
    {
        private readonly ITarefaRepositorio _tarefaRepositorio;

        public TarefasController(ITarefaRepositorio tarefaRepositorio)
        {
            _tarefaRepositorio = tarefaRepositorio;
        }

        // Index exibe lista todas as tarefas
        
        public async Task<IActionResult> Index()
        {
            try
            {
                // Busca todas as tarefas do repositório
                var tarefas = await _tarefaRepositorio.ObterTodasAsync();

                // Passa para a view
                return View(tarefas);
            }
            catch (Exception ex)
            {
                // Se der erro, retorna mensagem de erro
                ModelState.AddModelError("", $"Erro ao carregar tarefas: {ex.Message}");
                return View(new List<Tarefa>());
            }
        }

        /// Exibe os DETALHES de uma tarefa específica
       
        public async Task<IActionResult> Details(int? id)
        {
            // Se não passou ID, redireciona para Index
            if (id == null)
                return RedirectToAction("Index");

            // Busca a tarefa pelo ID
            var tarefa = await _tarefaRepositorio.ObterPorIdAsync(id.Value);

            // Se não encontrou erro 404
            if (tarefa == null)
                return NotFound();

            // Passa para a view
            return View(tarefa);
        }

        /// Exibe o formulário vazio para criar nova tarefa
        
        [HttpGet]
        public IActionResult Create()
        {
            // Retorna o formulário vazio
            return View();
        }

        /// Recebe os dados preenchidos e salva no banco
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Descricao,Data")] Tarefa tarefa)
        {
            try
            {
                // Valida se o modelo está correto
                if (ModelState.IsValid)
                {
                    // Define UsuarioId
                    // Depois será preenchido com o ID do usuário logado (Lucas é contigo)
                    tarefa.UsuarioId = 1; // ⚠️ TEMPORÁRIO - Lucas vai mudar isso

                    // Cria a tarefa no banco
                    await _tarefaRepositorio.CriarAsync(tarefa);

                    // Redireciona para Index
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao criar tarefa: {ex.Message}");
            }

            // Se houver erro, retorna o formulário com as validações
            return View(tarefa);
        }

        /// Exibe o formulário preenchido da tarefa com ID 5
        
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            // Se não passou ID redireciona para Index
            if (id == null)
                return RedirectToAction("Index");

            // Busca a tarefa
            var tarefa = await _tarefaRepositorio.ObterPorIdAsync(id.Value);

            // Se não encontrou, retorna 404
            if (tarefa == null)
                return NotFound();

            // Retorna o formulário preenchido
            return View(tarefa);
        }

        /// Recebe os dados editados e atualiza no banco
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,Data,StatusConcluida,UsuarioId")] Tarefa tarefa)
        {
            // Valida se é a tarefa certa
            if (id != tarefa.Id)
                return NotFound();

            try
            {
                // Valida o modelo
                if (ModelState.IsValid)
                {
                    // Atualiza a tarefa
                    await _tarefaRepositorio.AtualizarAsync(tarefa);

                    // Redireciona para Index
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao atualizar tarefa: {ex.Message}");
            }

            // Se houver erro, retorna o formulário
            return View(tarefa);
        }

        /// Exibe a tarefa antes de deletar
        
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            // Se não passou ID, redireciona para Index
            if (id == null)
                return RedirectToAction("Index");

            // Busca a tarefa
            var tarefa = await _tarefaRepositorio.ObterPorIdAsync(id.Value);

            // Se não encontrou, retorna 404
            if (tarefa == null)
                return NotFound();

            // Retorna a tarefa para confirmação
            return View(tarefa);
        }

        /// Realmente deleta a tarefa
        
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            try
            {
                // Deleta a tarefa
                var resultado = await _tarefaRepositorio.DeletarAsync(id.Value);

                // Se conseguiu deletar, retorna para Index
                if (resultado)
                    return RedirectToAction("Index");
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao deletar tarefa: {ex.Message}");
                return await Delete(id);
            }
        }
    }
}