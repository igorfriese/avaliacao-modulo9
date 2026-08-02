using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using avaliacao_modulo9.Data;
using avaliacao_modulo9.Models;
using System.Linq;
using System.Security.Claims;

namespace avaliacao_modulo9.Controllers
{
    [Authorize]
    public class TarefasController : Controller
    {
        private readonly ITarefaRepositorio _tarefaRepositorio;

        public TarefasController(ITarefaRepositorio tarefaRepositorio)
        {
            _tarefaRepositorio = tarefaRepositorio;
        }

        private int UsuarioIdLogado =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<IActionResult> Index(bool? status)
        {
            try
            {
                var tarefas = await _tarefaRepositorio.ObterTodasAsync();

                tarefas = tarefas.Where(t => t.UsuarioId == UsuarioIdLogado).ToList();

                if (status.HasValue)
                {
                    tarefas = tarefas.Where(t => t.StatusConcluida == status.Value).ToList();
                }

                return View(tarefas);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao carregar tarefas: {ex.Message}");
                return View(new List<Tarefa>());
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Index));

            var tarefa = await _tarefaRepositorio.ObterPorIdAsync(id.Value);

            if (tarefa == null || tarefa.UsuarioId != UsuarioIdLogado)
                return NotFound();

            return View(tarefa);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Descricao,Data,StatusConcluida")] Tarefa tarefa)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tarefa.UsuarioId = UsuarioIdLogado;
                    await _tarefaRepositorio.CriarAsync(tarefa);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao criar tarefa: {ex.Message}");
            }

            return View(tarefa);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Index));

            var tarefa = await _tarefaRepositorio.ObterPorIdAsync(id.Value);

            if (tarefa == null || tarefa.UsuarioId != UsuarioIdLogado)
                return NotFound();

            return View(tarefa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,Data,StatusConcluida,UsuarioId")] Tarefa tarefa)
        {
            if (id != tarefa.Id || tarefa.UsuarioId != UsuarioIdLogado)
                return NotFound();

            try
            {
                if (ModelState.IsValid)
                {
                    await _tarefaRepositorio.AtualizarAsync(tarefa);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao atualizar tarefa: {ex.Message}");
            }

            return View(tarefa);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Index));

            var tarefa = await _tarefaRepositorio.ObterPorIdAsync(id.Value);

            if (tarefa == null || tarefa.UsuarioId != UsuarioIdLogado)
                return NotFound();

            return View(tarefa);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Index));

            try
            {
                var resultado = await _tarefaRepositorio.DeletarAsync(id.Value);

                if (!resultado)
                    return NotFound();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao deletar tarefa: {ex.Message}");
                return await Delete(id);
            }
        }
    }
}