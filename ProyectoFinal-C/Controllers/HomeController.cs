using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_C.Models;
using System.Diagnostics;

namespace ProyectoFinal_C.Controllers
{   //Aqui redirijo a las vistas
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Registro()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult EnviadoRecuperar()
        {
            return View();
        }
        public IActionResult RegistroExitoso()
        {
            return View();
        }
        public IActionResult InicioSesionExitoso()
        {
            return View();
        }
        public IActionResult RecuperarContraseña()
        {
            return View();
        }
        public IActionResult RecuperarExitoso()
        {
            return View();
        }
        public IActionResult ErrorPersonalizado()
        {
            return View();
        }
        public IActionResult CambiarContrasena()
        {
            return View();
        }
    }
}