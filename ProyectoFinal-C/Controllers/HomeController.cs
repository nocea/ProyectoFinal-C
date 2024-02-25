using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoFinal_C.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Text;

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
        
        public async Task<IActionResult> MiCuenta()
        {

            ClaimsPrincipal claimUser = HttpContext.User;
            string nombreUsuario = "";
            Usuario usuario = null;
            if (claimUser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }
            string apiUrl1 = "https://localhost:7289/api/Controlador_Gestion/ObtenerUsuarioNombre/" + nombreUsuario;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl1);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Deserializar la respuesta JSON en una lista de usuarios
                        usuario = JsonConvert.DeserializeObject<Usuario>(responseBody);
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
            return View(usuario);
        }
        
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            string nombreUsuario = "";
            Usuario usuario = null;
            if (claimUser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimUser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }
            ViewData["nombreUsuario"]=nombreUsuario;
            return View();
        }
        
        public IActionResult ErrorPersonalizado()
        {
            return View();
        }
        
    }
}