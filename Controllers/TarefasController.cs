using Microsoft.AspNetCore.Mvc;

namespace avaliacao_modulo9.Controllers
{
    public class TarefasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
