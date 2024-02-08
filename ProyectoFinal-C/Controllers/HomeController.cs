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
            return View(usuarios);
        }
        public async Task<IActionResult> EditarUsuario()
        {
            int id;
            //Pilla el id de la url lo intenta convertir a string y lo guarda en "id"
            if (int.TryParse(Request.Query["id"], out id))
            {
                string apiUrl = $"https://localhost:7289/api/Controlador_Gestion/{id}";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(apiUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();

                            // Deserializar la respuesta JSON en un objeto de tipo Usuario
                            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(responseBody);

                            // Pasar el usuario a la vista de edición
                            return View(usuario);
                        }
                        else
                        {
                            // Manejar el caso en el que la solicitud a la API no sea exitosa
                            // Aquí puedes devolver una vista de error o manejar el error de otra manera
                            return NotFound();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejar excepciones si ocurren durante la solicitud HTTP
                        // Aquí puedes devolver una vista de error o manejar el error de otra manera
                        return View("Error");
                    }
                }
            }
            else
            {
                // Manejar el caso en el que la ID no sea válida
                // Aquí puedes devolver una vista de error o manejar el error de otra manera
                return BadRequest();
            }

        }
        public IActionResult EditarExitoso()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GuardarCambios(Usuario usuarioFormulario,string accion,string id)
        {
            int id_usuario = int.Parse(id);
            Usuario usuarioBBDD;
            if (accion == "guardar")
            {
                string apiUrl1 = $"https://localhost:7289/api/Controlador_Gestion/"+ id_usuario;
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(apiUrl1);
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            // Deserializar la respuesta JSON en un objeto de tipo Usuario
                            usuarioBBDD = JsonConvert.DeserializeObject<Usuario>(responseBody);
                        }
                        else
                        {
                            // Manejar el caso en el que la solicitud a la API no sea exitosa
                            // Aquí puedes devolver una vista de error o manejar el error de otra manera
                            return NotFound();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejar excepciones si ocurren durante la solicitud HTTP
                        // Aquí puedes devolver una vista de error o manejar el error de otra manera
                        return View("Error");
                    }
                }

                try
                {
                    var usuarioEditado = new Usuario
                    {
                        alias_usuario = usuarioFormulario.alias_usuario,
                        email_usuario = usuarioFormulario.email_usuario,
                        movil_usuario = usuarioFormulario.movil_usuario,
                        nombreCompleto_usuario = usuarioFormulario.nombreCompleto_usuario,
                        id_usuario = usuarioBBDD.id_usuario,
                        passwd_usuario=usuarioBBDD.passwd_usuario,
                        rol_usuario=usuarioBBDD.rol_usuario
                    };
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string apiUrl2 = "https://localhost:7289/api/Controlador_Gestion/EditarUsuario";
                        string jsonUsuario = JsonConvert.SerializeObject(usuarioEditado);
                        StringContent content = new StringContent(jsonUsuario, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await httpClient.PostAsync(apiUrl2, content);
                        if (response.IsSuccessStatusCode)
                        {
                            // Editarr exitoso
                            return RedirectToAction("EditarExitoso", "Home");
                        }
                        else if (response.StatusCode == HttpStatusCode.Conflict)//si hay algun mensaje de conflicto email/alias
                        {
                            //muestro la vista con el error
                            return View("ErrorPersonalizado", "Home");
                        }
                        else
                        {
                            // para otro error no controlado

                            return View("ErrorPersonalizado", "Home");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("ErrorPersonalizado", "Home");
                }
            }
            else if (accion == "borrar")
            {
                string apiUrl3 = $"https://localhost:7289/api/Controlador_Gestion/EliminarUsuario/"+id_usuario;
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.DeleteAsync(apiUrl3);
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            // Pasar el usuario a la vista de edición
                            return RedirectToAction("EditarExitoso", "Home");
                        }
                        else
                        {
                            // Manejar el caso en el que la solicitud a la API no sea exitosa
                            // Aquí puedes devolver una vista de error o manejar el error de otra manera
                            return NotFound();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejar excepciones si ocurren durante la solicitud HTTP
                        // Aquí puedes devolver una vista de error o manejar el error de otra manera
                        return RedirectToAction("ErrorPersonalizado", "Home");
                    }
                }
            }
            else { return RedirectToAction("ErrorPersonalizado", "Home"); }
        }
    }
}