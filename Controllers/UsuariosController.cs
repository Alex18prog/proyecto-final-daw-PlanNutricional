using Microsoft.AspNetCore.Mvc;
using NutriPlan.Data;
using NutriPlan.Models;
using NutriPlan.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace NutriPlan.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context) 
        { 
            _context = context; 
        }

        // GET: Usuarios/Registro
        public IActionResult Registro(string? planElegido) 
        { 
            // Instanciamos el modelo con el plan para que la vista lo reconozca
            var usuario = new Usuario { PlanElegido = planElegido };
            return View(usuario);
        }

        // POST: Usuarios/Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(Usuario usuario, string? planElegido)
        {
            if (ModelState.IsValid)
            {
                usuario.PlanElegido = planElegido;
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                
                HttpContext.Session.SetInt32("UsuarioId", usuario.IdUsuario);
                
                if (!string.IsNullOrEmpty(planElegido))
                {
                    await GenerarMenuInterno(usuario.IdUsuario);
                    // CAMBIO: En lugar de ir a MiPlan, vamos a una vista de Bienvenida
                    return RedirectToAction("Bienvenido", "Usuarios"); 
                }
                
                return RedirectToAction("SeleccionPlan", "Usuarios"); 
            }
            return View(usuario);
        }

        public IActionResult SeleccionPlan() => View();

        public async Task<IActionResult> GuardarPlan(string plan)
        {
            int? id = HttpContext.Session.GetInt32("UsuarioId");
            if (id == null) return RedirectToAction("Login");

            var usuario = await _context.Usuarios.FindAsync(id);
            
            if (usuario != null)
            {
                // 1. Actualizamos el nombre del plan en el perfil
                usuario.PlanElegido = plan;
                
                // 2. Opcional: Borrar el plan anterior para generar uno nuevo
                var planAnterior = await _context.PlanesSemanales
                    .FirstOrDefaultAsync(p => p.IdUsuario == id);
                    
                if (planAnterior != null)
                {
                    // Borramos las comidas del plan anterior para que no se mezclen
                    var comidasAnteriores = _context.PlanesComida
                        .Where(pc => pc.IdPlanSemanal == planAnterior.IdPlanSemanal);
                    _context.PlanesComida.RemoveRange(comidasAnteriores);
                    _context.PlanesSemanales.Remove(planAnterior);
                    await _context.SaveChangesAsync();
                }

                // 3. Generamos el nuevo menú (el método que ya creamos)
                await GenerarMenuInterno(id.Value);
                
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Bienvenido");
        }

        public IActionResult Planes() => View();

        public IActionResult DetallePlan(string nombre)
        {
            ViewData["Plan"] = nombre;
            return View();
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string contrasena)
        {
            // Buscamos al usuario por correo y contraseña
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.CorreoElectronico == correo && u.ContrasenaHash == contrasena);

            if (usuario != null)
            {
                // Guardamos el ID en la sesión
                HttpContext.Session.SetInt32("UsuarioId", usuario.IdUsuario);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Correo o contraseña incorrectos");
            return View();
        }

       public async Task<IActionResult> MiPlan()
        {
            int? userId = HttpContext.Session.GetInt32("UsuarioId");
            if (userId == null) return RedirectToAction("Login", "Usuarios");

            var plan = await _context.PlanesSemanales
                .Where(p => p.IdUsuario == userId)
                .OrderByDescending(p => p.IdPlanSemanal) 
                .FirstOrDefaultAsync();

            if (plan == null) 
            {
                await GenerarMenuInterno(userId.Value);
                plan = await _context.PlanesSemanales.FirstOrDefaultAsync(p => p.IdUsuario == userId);
            }

            var miPlan = await _context.PlanesComida
                .Where(pc => pc.IdPlanSemanal == plan.IdPlanSemanal)
                .Join(_context.Recetas, 
                    pc => pc.IdReceta, 
                    r => r.IdReceta,
                    (pc, r) => new MiPlanViewModel {
                        DiaSemana = pc.DiaSemana, 
                        MomentoDia = pc.MomentoDia,
                        NombreReceta = r.Nombre, 
                        Calorias = r.Calorias
                    })
                .ToListAsync();

            return View(miPlan);
        }

        [HttpPost]
        public async Task<IActionResult> SeleccionarPlan(int idPlanElegido)
        {
            int? userId = HttpContext.Session.GetInt32("UsuarioId");
            
            // 1. Crear el registro en PlanSemanal
            var nuevoPlanSemanal = new PlanSemanal { 
                IdUsuario = userId.Value, 
                FechaInicio = DateTime.Now 
            };
            _context.PlanesSemanales.Add(nuevoPlanSemanal);
            await _context.SaveChangesAsync();

            // 2. LÓGICA AUTOMÁTICA: Rellenar con platos predefinidos
            var dias = new[] { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };
            var momentos = new[] { "Desayuno", "Comida", "Cena" };

            foreach (var dia in dias) {
                foreach (var momento in momentos) {
                    var receta = _context.Recetas.OrderBy(r => Guid.NewGuid()).FirstOrDefault();
                    
                    var planComida = new PlanComida {
                        IdPlanSemanal = nuevoPlanSemanal.IdPlanSemanal,
                        IdReceta = receta.IdReceta,
                        DiaSemana = dia,
                        MomentoDia = momento
                    };
                    _context.PlanesComida.Add(planComida);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("MiPlan", "Usuarios");
        }

        private async Task GenerarMenuInterno(int userId)
        {
            // 1. LIMPIEZA: Borramos planes antiguos
            var planExistente = await _context.PlanesSemanales
                .FirstOrDefaultAsync(p => p.IdUsuario == userId);

            if (planExistente != null)
            {
                var comidas = _context.PlanesComida.Where(c => c.IdPlanSemanal == planExistente.IdPlanSemanal);
                _context.PlanesComida.RemoveRange(comidas);
                _context.PlanesSemanales.Remove(planExistente);
                await _context.SaveChangesAsync();
            }

            // 2. OBTENEMOS EL USUARIO
            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null || string.IsNullOrEmpty(usuario.PlanElegido)) return;

            string planBuscado = usuario.PlanElegido.Trim();

            // 3. CREAMOS EL NUEVO PLAN
            var nuevoPlan = new PlanSemanal { IdUsuario = userId, FechaInicio = DateTime.Now };
            _context.PlanesSemanales.Add(nuevoPlan);
            await _context.SaveChangesAsync();

            // 4. ESTRUCTURA
            var dias = new[] { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };
            var momentos = new[] { "Desayuno", "Comida", "Cena" };

            // Traemos TODAS las recetas del plan a memoria para poder mezclarlas fácilmente
            var todasLasRecetasDelPlan = await _context.Recetas
                .Where(r => r.PlanAsociado == planBuscado)
                .ToListAsync();

            foreach (var momento in momentos)
            {
                // Filtramos las recetas de este momento (ej. todos los desayunos)
                // y las desordenamos aleatoriamente
                var recetasMomento = todasLasRecetasDelPlan
                    .Where(r => r.MomentoDia == momento)
                    .OrderBy(x => Guid.NewGuid()) 
                    .ToList();

                for (int i = 0; i < dias.Length; i++)
                {
                    // Seleccionamos una receta de la lista (usamos el índice para variar cada día)
                    // Si hay 7 recetas, cada día de la semana tendrá una diferente
                    var recetaSeleccionada = recetasMomento[i % recetasMomento.Count];

                    _context.PlanesComida.Add(new PlanComida
                    {
                        IdPlanSemanal = nuevoPlan.IdPlanSemanal,
                        IdReceta = recetaSeleccionada.IdReceta,
                        DiaSemana = dias[i],
                        MomentoDia = momento
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
        public IActionResult Bienvenido()
        {
            int? userId = HttpContext.Session.GetInt32("UsuarioId");
            var usuario = _context.Usuarios.Find(userId);
            
            ViewBag.NombrePlan = usuario?.PlanElegido ?? "tu plan";
            
            return View();
        }

        public async Task<IActionResult> Perfil()
        {
            int? userId = HttpContext.Session.GetInt32("UsuarioId");
            if (userId == null) return RedirectToAction("Login");

            var usuario = await _context.Usuarios.FindAsync(userId);
            return View(usuario); 
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Limpia toda la sesión
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ListaCompra()
        {
            int? userId = HttpContext.Session.GetInt32("UsuarioId");
            if (userId == null) return RedirectToAction("Login");

            // 1. Buscamos el plan del usuario
            var plan = await _context.PlanesSemanales.FirstOrDefaultAsync(p => p.IdUsuario == userId);
            if (plan == null) return RedirectToAction("SeleccionPlan");

            // 2. Traemos todas las recetas de su plan actual
            var ingredientes = await _context.PlanesComida
                .Where(pc => pc.IdPlanSemanal == plan.IdPlanSemanal)
                .Join(_context.Recetas, pc => pc.IdReceta, r => r.IdReceta, (pc, r) => r.Ingredientes)
                .ToListAsync();

            // 3. Unimos todo en una lista única 
            var listaCompleta = string.Join(", ", ingredientes).Split(',').Select(i => i.Trim()).Distinct().ToList();

            return View(listaCompleta);
        }

        public async Task<IActionResult> DetalleReceta(string nombre)
        {
            if (string.IsNullOrEmpty(nombre)) return RedirectToAction("MiPlan");

            var receta = await _context.Recetas
                .FirstOrDefaultAsync(r => r.Nombre == nombre);

            if (receta == null) return NotFound();

            return View(receta);
        }
    }
}