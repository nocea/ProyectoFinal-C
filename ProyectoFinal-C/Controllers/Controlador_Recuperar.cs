using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoFinal_C.Models;
using System.Net;
using System.Net.Http;
using System.Text;


namespace ProyectoFinal_C.Controllers
{
    public class Controlador_Recuperar : Controller
    {
        [HttpPost]
        public async Task<IActionResult> RecuperarContrasena(Usuario usuarioRecuperar)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string apiUrl = "https://localhost:7289/api/Controlador_Recuperar/RecuperarContrasena";

                    
                    string jsonUsuario = JsonConvert.SerializeObject(usuarioRecuperar);

                    
                    StringContent content = new StringContent(jsonUsuario, Encoding.UTF8, "application/json");

                    
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        
                        return RedirectToAction("EnviadoRecuperar", "Home");
                    }
                    else if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        
                        ModelState.AddModelError(string.Empty, errorMessage);
                        
                        return View("~/Views/Home/RecuperarContraseña.cshtml", usuarioRecuperar);
                    }
                    else
                    {
                        
                        return View("ErrorPersonalizado", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPersonalizado", "Home");
            }
        }     
        [HttpPost]
        public async Task<IActionResult> CambiarContrasena(Usuario usuarioContrasenaNueva,string email, string token)
        {
            try
            {
                await Console.Out.WriteLineAsync(email);
                await Console.Out.WriteLineAsync(token);
                using (HttpClient httpClient = new HttpClient())
                {
                    string apiUrl = "https://localhost:7289/api/Controlador_Recuperar/CambiarContrasena";
                    string jsonUsuario = JsonConvert.SerializeObject(usuarioContrasenaNueva);
                    StringContent content = new StringContent(jsonUsuario, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("CambiarContrasenaExitoso", "Home");
                    }
                    else if (response.StatusCode == HttpStatusCode.Conflict)
                    {

                        return View("ErrorPersonalizado", "Home");
                    }
                    else
                    {
                        return View("ErrorPersonalizado", "Home");
                    }
                }
            }catch (Exception ex)
            {
                return View("ErrorPersonalizado", "Home");
            }
        }
    }
}
