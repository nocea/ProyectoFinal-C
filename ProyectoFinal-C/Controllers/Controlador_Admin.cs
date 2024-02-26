using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoFinal_C.Models;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace ProyectoFinal_C.Controllers
{
    [AdminAuthorizationFilter]
    public class Controlador_Admin : Controller
    {
        public IActionResult RegistroAdmin()
        {
            return View();
        }
        public IActionResult RegistroExitosoAdmin()
        {
            return View();
        }
        /// <summary>
        /// Método para mostrar el index de admin si entras como admin
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> IndexAdmin()
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
        /// <summary>
        /// Método para registrar un usuario como admin
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RegistrarUsuario(Usuario usuario)
        {
            try
            {
                // Creo un usuario de la misma forma que lo tiene la API
                var nuevoUsuario = new Usuario
                {
                    nombreCompleto_usuario = usuario.nombreCompleto_usuario,
                    rol_usuario = "USUARIO",
                    movil_usuario = usuario.movil_usuario,
                    alias_usuario = usuario.alias_usuario,
                    passwd_usuario = usuario.passwd_usuario,
                    email_usuario = usuario.email_usuario,
                };

                // Sirve para recibir y hacer peticiones http a la api
                using (HttpClient httpClient = new HttpClient())
                {
                    string apiUrl = "https://localhost:7289/api/Controlador_Registro";

                    // para convertir el usuario a json y poder pasarselo a la API
                    string jsonUsuario = JsonConvert.SerializeObject(nuevoUsuario);

                    // contenido de la petición
                    StringContent content = new StringContent(jsonUsuario, Encoding.UTF8, "application/json");

                    // peticion con la url y el contenido que se le manda al post y guarda el mensaje de respuesta
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                    // con el mensaje de respuesta muestra la vista
                    if (response.IsSuccessStatusCode)// si se ha registrado correctamente me manda a la vista de registro exitoso
                    {
                        // Registro exitoso
                        return RedirectToAction("RegistroExitosoAdmin", "Controlador_Admin");
                    }
                    else
                    {
                        //guardo el mensaje de error que me ha mandado
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        //muestro la vista con el error
                        return View("~/Views/Home/ErrorPersonalizado.cshtml", errorMessage);
                    }

                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPersonalizado", "Home");
            }
        }
        /// <summary>
        /// Método para mostrar la vista de gestion de usuarios
        /// </summary>
        /// <returns></returns>
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
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        return View("~/Views/Home/ErrorPersonalizado.cshtml", errorMessage);
                    }

                }
                catch (Exception ex)
                {
                    return View("ErrorPersonalizado", "Home");
                }
            }
            return View(usuarios);
        }
        /// <summary>
        /// Método para mostrar la vista de gestión de posts
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GestionPosts()
        {
            string apiUrl = "https://localhost:7289/api/Controlador_Gestion/AllPosts";
            List<Post> posts = new List<Post>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Deserializar la respuesta JSON en una lista de usuarios
                        posts = JsonConvert.DeserializeObject<List<Post>>(responseBody);
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        return View("~/Views/Home/ErrorPersonalizado.cshtml", errorMessage);
                    }

                }
                catch (Exception ex)
                {
                    return View("ErrorPersonalizado", "Home");
                }
            }
            return View(posts);
        }
        /// <summary>
        /// Método para borrar un post por su id
        /// </summary>
        /// <param name="idPost"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> BorrarPost(string idPost)
        {

            string apiUrl3 = $"https://localhost:7289/api/Controlador_Gestion/BorrarPost/" + idPost;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.DeleteAsync(apiUrl3);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        return RedirectToAction("GestionPosts", "Controlador_Admin");
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        return View("~/Views/Home/ErrorPersonalizado.cshtml", errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("ErrorPersonalizado", "Home");
                }
            }
        }
        /// <summary>
        /// Método para mostrar la vista de editar usuarios
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> EditarUsuario()
        {
            int id;
            string errorMessage;
            //Pilla el id de la url lo intenta convertir a string y lo guarda en "id"
            if (int.TryParse(Request.Query["id"], out id))
            {
                string apiUrl = $"https://localhost:7289/api/Controlador_Gestion/{id}";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(apiUrl);
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var errorObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
                        errorMessage = errorObject.mensaje;
                        if (response.IsSuccessStatusCode)
                        {
                            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(responseBody);
                            return View(usuario);
                        }
                        else
                        {

                            return View("~/Views/Home/ErrorPersonalizado.cshtml", errorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = "Error al editar el usuario";
                        return View("~/Views/Home/ErrorPersonalizado.cshtml", errorMessage);
                    }
                }
            }
            else
            {
                errorMessage = "Error al convertir el usuario";
                return View("~/Views/Home/ErrorPersonalizado.cshtml", errorMessage);
            }
        }
        public IActionResult EditarExitoso()
        {
            return View();
        }

        /// <summary>
        /// Método para guardar cambios o borrar dependiendo de la accion
        /// </summary>
        /// <param name="usuarioFormulario"></param>
        /// <param name="accion"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GuardarCambios(Usuario usuarioFormulario, string accion, string id)
        {
            try
            {
                int id_usuario = int.Parse(id);
                Usuario usuarioBBDD;
                if (accion == "guardar")//si es guardar
                {
                    string apiUrl1 = $"https://localhost:7289/api/Controlador_Gestion/" + id_usuario;
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
                        var usuarioEditado = new Usuario//camnbio los datros del usuario
                        {
                            alias_usuario = usuarioFormulario.alias_usuario,
                            email_usuario = usuarioFormulario.email_usuario,
                            movil_usuario = usuarioFormulario.movil_usuario,
                            nombreCompleto_usuario = usuarioFormulario.nombreCompleto_usuario,
                            id_usuario = usuarioBBDD.id_usuario,
                            passwd_usuario = usuarioBBDD.passwd_usuario,
                            rol_usuario = usuarioBBDD.rol_usuario
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
                                return RedirectToAction("EditarExitoso", "Controlador_Admin");
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
                else if (accion == "borrar")//si es borrar
                {
                    string apiUrl3 = $"https://localhost:7289/api/Controlador_Gestion/EliminarUsuario/" + id_usuario;
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            HttpResponseMessage response = await client.DeleteAsync(apiUrl3);
                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                
                                return RedirectToAction("EditarExitoso", "Controlador_Admin");
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
            catch (Exception ex)
            {
                return View("ErrorPersonalizado", "Home");
            }


    }
    }
}
