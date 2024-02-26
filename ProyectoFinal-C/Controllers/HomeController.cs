using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ProyectoFinal_C.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Reflection;
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
        [HttpPost]
        public async Task<IActionResult> CambiarImagen(Usuario usuario)
        {
            string errorMessage = null;
            try
            {
                if (usuario.ImagenFile != null && usuario.ImagenFile.Length > 0)
                {
                    // Leer la imagen en un array de bytes
                    using (var memoryStream = new MemoryStream())
                    {
                        await usuario.ImagenFile.CopyToAsync(memoryStream);
                        usuario.imagen_usuario = memoryStream.ToArray();
                    }
                }
                var nuevoUsuario = new Usuario
                {
                    id_usuario = usuario.id_usuario,
                    imagen_usuario=usuario.imagen_usuario                    
                };
                using (HttpClient httpClient = new HttpClient())
                {
                    string apiUrl = "https://localhost:7289/api/Controlador_Usuario/CambiarImagen";

                    // para convertir el usuario a json y poder pasarselo a la API
                    string jsonUsuario = JsonConvert.SerializeObject(nuevoUsuario);

                    // contenido de la petición
                    StringContent content = new StringContent(jsonUsuario, Encoding.UTF8, "application/json");

                    // peticion con la url y el contenido que se le manda al post y guarda el mensaje de respuesta
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var errorObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    if (errorObject != null)
                    {
                        errorMessage = errorObject.mensaje;
                    }
                    // con el mensaje de respuesta muestra la vista
                    if (response.IsSuccessStatusCode)// si se ha registrado correctamente me manda a la vista de registro exitoso
                    {
                        // Registro exitoso
                        return RedirectToAction("MiCuenta", "Home");
                    }
                    else if (response.StatusCode == HttpStatusCode.Conflict)//si hay algun mensaje de conflicto email/alias
                    {
                        //muestro la vista con el error
                        return View("~/Views/Home/ErrorPersonalizado.cshtml", errorMessage);
                    }
                    else
                    {
                        // para otro error no controlado

                        return View("~/Views/Home/ErrorPersonalizado.cshtml", errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                return View("~/Views/Home/ErrorPersonalizado.cshtml", errorMessage);
            }
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
            ViewData["nombreUsuario"] = nombreUsuario;
            return View();
        }

        public IActionResult ErrorPersonalizado()
        {
            return View();
        }
        public async Task<IActionResult> ParaTiAsync()
        {
            string apiUrl1 = "https://localhost:7289/api/Controlador_Usuario";
            List<Post> posts = new List<Post>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl1);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserializar la respuesta JSON en una lista de usuarios
                    posts = JsonConvert.DeserializeObject<List<Post>>(responseBody);
                }
                else
                {
                    Console.WriteLine($"Error al obtener los usuarios. Código de estado: {response.StatusCode}");
                }
            }
            return View(posts);
        }
        public async Task<IActionResult> CrearPostAsync()
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
        [HttpPost]
        public async Task<IActionResult> NuevoPost(Post post)
        {

            string errorMessage = null;
            try
            {
                if (post.ImagenFile != null && post.ImagenFile.Length > 0)
                {
                    // Leer la imagen en un array de bytes
                    using (var memoryStream = new MemoryStream())
                    {
                        await post.ImagenFile.CopyToAsync(memoryStream);
                        post.imagen_post = memoryStream.ToArray();
                    }
                }
                var nuevoPost = new Post
                {
                    titulo_post = post.titulo_post,
                    imagen_post = post.imagen_post,
                    pie_post = post.pie_post,
                    UsuarioId = post.UsuarioId,
                };
                using (HttpClient httpClient = new HttpClient())
                {
                    string apiUrl = "https://localhost:7289/api/Controlador_Usuario";

                    // para convertir el usuario a json y poder pasarselo a la API
                    string jsonUsuario = JsonConvert.SerializeObject(post);

                    // contenido de la petición
                    StringContent content = new StringContent(jsonUsuario, Encoding.UTF8, "application/json");

                    // peticion con la url y el contenido que se le manda al post y guarda el mensaje de respuesta
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var errorObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    if (errorObject != null)
                    {
                        errorMessage = errorObject.mensaje;
                    }
                    // con el mensaje de respuesta muestra la vista
                    if (response.IsSuccessStatusCode)// si se ha registrado correctamente me manda a la vista de registro exitoso
                    {
                        // Registro exitoso
                        return RedirectToAction("ParaTi", "Home");
                    }
                    else if (response.StatusCode == HttpStatusCode.Conflict)//si hay algun mensaje de conflicto email/alias
                    {
                        //muestro la vista con el error
                        return View("~/Views/Home/ErrorPersonalizado.cshtml", errorMessage);
                    }
                    else
                    {
                        // para otro error no controlado

                        return View("~/Views/Home/ErrorPersonalizado.cshtml", errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                return View("~/Views/Home/ErrorPersonalizado.cshtml", errorMessage);
            }
        }
        public async Task<IActionResult> ComentarPostAsync(string idPost)
        {
            
            string apiUrl1 = "https://localhost:7289/api/Controlador_Usuario/AllComentarios/"+ idPost;
            List<Comentario> comentarios = new List<Comentario>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl1);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserializar la respuesta JSON en una lista de usuarios
                    comentarios = JsonConvert.DeserializeObject<List<Comentario>>(responseBody);
                    ViewBag.ListaComentarios = comentarios;
                }
                else
                {
                    Console.WriteLine($"Error al obtener los usuarios. Código de estado: {response.StatusCode}");
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CrearComentario(Comentario comentario)
        {
            var errorMessage="";
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
                    using (HttpClient httpClient = new HttpClient())
                    {
                        comentario.UsuarioId = usuario.id_usuario;
                        string apiUrl = "https://localhost:7289/api/Controlador_Usuario/CrearComentario";

                        // para convertir el usuario a json y poder pasarselo a la API
                        string jsoncomentario = JsonConvert.SerializeObject(comentario);

                        // contenido de la petición
                        StringContent content = new StringContent(jsoncomentario, Encoding.UTF8, "application/json");

                        // peticion con la url y el contenido que se le manda al post y guarda el mensaje de respuesta
                        HttpResponseMessage response1 = await httpClient.PostAsync(apiUrl, content);
                        string responseBody = await response1.Content.ReadAsStringAsync();
                        var errorObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
                        if (errorObject != null)
                        {
                            errorMessage = errorObject.mensaje;
                        }
                        // con el mensaje de respuesta muestra la vista
                        if (response.IsSuccessStatusCode)// si se ha registrado correctamente me manda a la vista de registro exitoso
                        {
                            // Registro exitoso
                            return RedirectToAction("ParaTi", "Home");
                        }
                        else if (response.StatusCode == HttpStatusCode.Conflict)//si hay algun mensaje de conflicto email/alias
                        {
                            //muestro la vista con el error
                            return View("~/Views/Home/ErrorPersonalizado.cshtml", errorMessage);
                        }
                        else
                        {
                            // para otro error no controlado

                            return View("~/Views/Home/ErrorPersonalizado.cshtml", errorMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return View("~/Views/Home/ErrorPersonalizado.cshtml", errorMessage);
                }
            }
            }
    }
}