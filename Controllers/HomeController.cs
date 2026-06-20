using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NutriPlan.Models;
using NutriPlan.Data; 
using Microsoft.EntityFrameworkCore; 

namespace NutriPlan.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context; 

    // Modificamos el constructor para recibir el AppDbContext
    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> SembrarRecetas()
    {
        if (await _context.Recetas.AnyAsync()) return Content("Ya hay recetas en la base de datos.");

        var recetasIniciales = new List<Receta>
        {
            new Receta { Nombre = "Avena con Frutas", Calorias = 300 },
            new Receta { Nombre = "Ensalada de Pollo", Calorias = 500 },
            new Receta { Nombre = "Salmón a la plancha", Calorias = 600 },
            new Receta { Nombre = "Yogur con nueces", Calorias = 250 },
            new Receta { Nombre = "Pasta integral", Calorias = 550 },
            new Receta { Nombre = "Tortilla francesa", Calorias = 200 },
            new Receta { Nombre = "Pechuga con verduras", Calorias = 400 }
        };

        _context.Recetas.AddRange(recetasIniciales);
        await _context.SaveChangesAsync();

        return Content("¡Recetas creadas correctamente! Ya puedes probar el flujo de generación de planes.");
    }

    public IActionResult Index() => View();
    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}