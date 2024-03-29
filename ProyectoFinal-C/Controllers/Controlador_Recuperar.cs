﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoFinal_C.Models;
using System.Net;
using System.Net.Http;
using System.Text;


namespace ProyectoFinal_C.Controllers
{
    public class Controlador_Recuperar : Controller
    {
        public IActionResult EnviadoRecuperar()
        {
            return View();
        }
        public IActionResult RecuperarContraseña()
        {
            return View();
        }
        public IActionResult CambiarContrasena()
        {
            return View();
        }
        public IActionResult CambiarContrasenaExitoso()
        {
            return View();
        }
        /// <summary>
        /// Método que recibe un email y manda un correo a ese email y asi poder cambiar la contraseña
        /// </summary>
        /// <param name="usuarioRecuperar"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RecuperarContrasena(Usuario usuarioRecuperar)
        {
            try
            {
                var nuevoUsuario = new Usuario
                {
                    nombreCompleto_usuario = "",
                    rol_usuario = "",
                    movil_usuario = 0,
                    alias_usuario = "",
                    passwd_usuario = "",
                    email_usuario = usuarioRecuperar.email_usuario,                 
                };
                using (HttpClient httpClient = new HttpClient())
                {
                    string apiUrl = "https://localhost:7289/api/Controlador_Recuperar/RecuperarContrasena";

                    
                    string jsonUsuario = JsonConvert.SerializeObject(nuevoUsuario);

                    
                    StringContent content = new StringContent(jsonUsuario, Encoding.UTF8, "application/json");
                    
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        
                        return RedirectToAction("EnviadoRecuperar", "Controlador_Recuperar");
                    }
                    else
                    {
                        
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        
                        
                        
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
        /// Método para cambiar la contraseña si es valido el token y por el enlace que le he mandado anteriormente
        /// </summary>
        /// <param name="usuarioContrasenaNueva"></param>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CambiarContrasena(Usuario usuarioContrasenaNueva,string email, string token)
        {
            try
            {   //Guardo el usuario del form
                var usuarioEmailPasswdToken = new Usuario
                {
                    nombreCompleto_usuario = "",
                    rol_usuario = "",
                    movil_usuario = 0,
                    alias_usuario = "",
                    passwd_usuario = usuarioContrasenaNueva.passwd_usuario,
                    email_usuario = email,
                };
                using (HttpClient httpClient = new HttpClient())
                {
                    string apiUrl = "https://localhost:7289/api/Controlador_Recuperar/CambiarContrasena/"+token;
                    string jsonUsuario = JsonConvert.SerializeObject(usuarioEmailPasswdToken);
                    StringContent content = new StringContent(jsonUsuario, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("CambiarContrasenaExitoso", "Controlador_Recuperar");
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
