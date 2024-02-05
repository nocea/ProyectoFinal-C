using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_C.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace ProyectoFinal_C.Controllers
{   //Aqui redirijo a las vistas
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult InicioSesionExitoso()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            string nombreUsuario = "";
            if (claimUser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }
            ViewData["nombreUsuario"] = nombreUsuario;
            return View();
        }
        public IActionResult ErrorPersonalizado()
        {
            return View();
        }
        
        
    }
}