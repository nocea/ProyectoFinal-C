using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoFinal_C.Models;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ProyectoFinal_C.Controllers
{
    public class Controlador_Login : Controller
    {
        
        [HttpPost]
        public async Task<IActionResult> InicioSesion(Usuario usuarioLogin)
        {
            try
            {   //Guardo el usuario para el login
                var nuevoUsuario = new Usuario
                {
                    nombreCompleto_usuario = usuarioLogin.nombreCompleto_usuario,
                    rol_usuario = "rol_usuario",
                    movil_usuario = usuarioLogin.movil_usuario,
                    alias_usuario = usuarioLogin.alias_usuario,
                    passwd_usuario = usuarioLogin.passwd_usuario,
                    email_usuario = usuarioLogin.email_usuario,

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
                    
                    

                    // si ha encontrado todo devuelve una respuesta exitosa
                    if (response.IsSuccessStatusCode)
                    {
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
                        return View("~/Views/Home/Login.cshtml", usuarioLogin);
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
    }
}
