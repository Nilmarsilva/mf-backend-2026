using Microsoft.AspNetCore.Mvc;
using mf_backend_2026.Models;
using Microsoft.EntityFrameworkCore;

namespace mf_backend_2026.Controllers
{
    public class VeiculosController : Controller
    {
        private readonly AppDbContext _context;
        public VeiculosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dados = await _context.Veiculos.ToListAsync();

            return View(dados); 
        
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Veiculo veiculo)
        {
            if (ModelState.IsValid)
            {
                _context.Veiculos.Add(veiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }




            // Retorna a view com o modelo para que mensagens de validação e valores preenchidos
            // sejam preservados quando ModelState for inválido.
            return View(veiculo);
        }

    }
}
