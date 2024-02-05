using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoFinal_C.Models;
using System.Net;
using System.Net.Http;
using System.Text;
//Autentificacion por cookies
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace ProyectoFinal_C.Controllers
{
    public class Controlador_Login : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> InicioSesion(Usuario usuarioLogin)
        {
            try
            {   //Guardo el usuario del form
                var nuevoUsuario = new Usuario
                {
                    nombreCompleto_usuario = "",
                    rol_usuario = "",
                    movil_usuario = 0,
                    alias_usuario = "",
                    passwd_usuario = usuarioLogin.passwd_usuario,
                    email_usuario = usuarioLogin.email_usuario,
                    token_usuario=""

                };
               
                using (HttpClient client = new HttpClient())
                {
                    string apiBaseUrl = "https://localhost:7289/api/Controlador_Login";
                    //lo paso a json para poder mandarselo a la api
                    string jsonUsuario = JsonConvert.SerializeObject(nuevoUsuario);

                    // hago la solicitud a la api
                    StringContent content = new StringContent(jsonUsuario, Encoding.UTF8, "application/json");
                    //le envio la solicitud y espero a la respuesta
                    HttpResponseMessage response = await client.PostAsync(apiBaseUrl, content);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    UsuarioResponse usuarioResponse = JsonConvert.DeserializeObject<UsuarioResponse>(responseBody);


                    // si ha encontrado todo devuelve una respuesta exitosa
                    if (response.IsSuccessStatusCode&&usuarioResponse.Usuario != null)
                    {
                        Usuario usuarioEncontrado = usuarioResponse.Usuario;
                        Console.WriteLine(responseBody);
                        List<Claim> claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name,usuarioEncontrado.nombreCompleto_usuario)
                        };
                        Console.WriteLine(usuarioEncontrado.nombreCompleto_usuario);
                        ClaimsIdentity identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                        AuthenticationProperties authenticationProperties = new AuthenticationProperties()
                        {
                            AllowRefresh = true
                        };
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(identity),
                            authenticationProperties
                            );
                        // muestro al vista de inicio sesion exitoso
                        return RedirectToAction("InicioSesionExitoso", "Home");
                    }
                    else if(response.StatusCode == HttpStatusCode.Conflict)
                    {
                        //guardo el mensaje de error que me ha mandado
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        //lo añado al modelState para poder mostrarlo por la vista
                        ModelState.AddModelError(string.Empty, errorMessage);
                        //muestro la vista con el error
                        return View("~/Views/Controlador_Login/Login.cshtml", usuarioLogin);
                    }
                    else
                    {                       
                        return RedirectToAction("ErrorPersonalizado", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                //Para cualquier error me manda a la vista de error personalizado
                return RedirectToAction("ErrorPersonalizado", "Home");
            }
        }
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Controlador_Login");
        }
    }
}
