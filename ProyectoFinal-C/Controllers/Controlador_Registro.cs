﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoFinal_C.Models;
using System.Net;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using NuGet.Common;

namespace ProyectoFinal_C.Controllers
{
    
    public class Controlador_Registro : Controller
    {
        public IActionResult Registro()
        {
            return View();
        }
        public IActionResult RegistroExitoso()
        {
            return View();
        }
        public async Task<IActionResult> ConfirmacionRegistro(string email)
        {
            try
            {   
                using (HttpClient httpClient = new HttpClient())
                {
                    string apiUrl = "https://localhost:7289/api/Controlador_Registro/ConfirmacionRegistro/" + email;
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, null);
                    if (response.IsSuccessStatusCode)
                    {
                        return View();
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
            }
            catch (Exception ex)
            {
                return View("ErrorPersonalizado", "Home");
            }
        }
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
                        return RedirectToAction("RegistroExitoso","Controlador_registro"); 
                    }
                    else if (response.StatusCode == HttpStatusCode.Conflict)//si hay algun mensaje de conflicto email/alias
                    {   
                        //guardo el mensaje de error que me ha mandado
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        //lo añado al modelState para poder mostrarlo por la vista
                        ModelState.AddModelError(string.Empty, errorMessage);
                        //muestro la vista con el error
                        return View("~/Views/Controlador_Registro/Registro.cshtml", usuario);
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
    }
}
