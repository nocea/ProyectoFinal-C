using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoFinal_C.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace ProyectoFinal_C.Controllers
{   //Aqui redirijo a las vistas
    //En estas vistas se guardara la sesion por la siguiente etiqueta
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
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
        public IActionResult IndexAdmin()
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
        public async Task<IActionResult> GestionUsuarios()
        {
            string apiUrl = "https://localhost:7289/api/Controlador_Gestion";
            List<Usuario> usuarios = new List<Usuario>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Deserializar la respuesta JSON en una lista de usuarios
                        usuarios = JsonConvert.DeserializeObject<List<Usuario>>(responseBody);
                    }
                    else
                    {
                        Console.WriteLine($"Error al obtener los usuarios. Código de estado: {response.StatusCode}");
                    }
                    
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
            return View("GestionUsuarios", usuarios);
        }
        public IActionResult EditarUsuario()
        {
            return View();
        }
        
    }
}