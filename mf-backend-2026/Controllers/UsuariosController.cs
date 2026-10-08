
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mf_backend_2026.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

public class UsuariosController : Controller
{
    private readonly AppDbContext _context;

    public ClaimsPrincipal ClaimsPrincipal { get; private set; }

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: USUARIOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Usuarios.ToListAsync());
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(Usuario usuario)
    {
         if (usuario == null || usuario.Id == 0)
        {
            ViewBag.Mensagem = "Usuário e/ou senha incorretos!";
            return View();
        }

        var dados = await _context.Usuarios.FindAsync(usuario.Id);

        if (dados == null)
        {
            ViewBag.Mensagem = "Usuário e/ou senha incorretos!";
            return View();
        }

        bool senhaok = BCrypt.Net.BCrypt.Verify(usuario.Senha, dados.Senha);

        if (!senhaok)
        {
            ViewBag.Mensagem = "Usuário e/ou senha incorretos!";
            return View();
        }

        // Credenciais válidas: criar claims e autenticar
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, dados.Nome),
            new Claim(ClaimTypes.NameIdentifier, dados.Id.ToString()),
            new Claim(ClaimTypes.Role, dados.Perfil.ToString())
        };
        var claimsIdentity = new ClaimsIdentity(claims, "Login");
        var principal = new ClaimsPrincipal(claimsIdentity);

        var props = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTime.UtcNow.AddMinutes(10),
            IsPersistent = true
        };

        // usar o mesmo esquema de cookie ao autenticar
        await HttpContext.SignInAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

        HttpContext.Session.SetInt32("UsuarioId", dados.Id);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Usuarios");
    }

    // GET: USUARIOS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(m => m.Id == id);
        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    // GET: USUARIOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: USUARIOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nome,Email,Senha,Perfil")] Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
            _context.Add(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(usuario);
    }

    // GET: USUARIOS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }
        return View(usuario);
    }

    // POST: USUARIOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nome,Email,Senha,Perfil")] Usuario usuario)
    {
        if (id != usuario.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(usuario);
    }

    // GET: USUARIOS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(m => m.Id == id);
        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    // POST: USUARIOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UsuarioExists(int? id)
    {
        return _context.Usuarios.Any(e => e.Id == id);
    }
}
